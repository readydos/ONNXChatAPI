using Microsoft.OpenApi;
using OnnxChatApi.Options;
using OnnxChatApi.Services;
using System.Net.WebSockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls(builder.Configuration["Urls"] ?? "http://0.0.0.0:5000");

builder.Services.Configure<ONNXGenAIOptions>(
    builder.Configuration.GetSection(ONNXGenAIOptions.SectionName));

builder.Services.AddSingleton<IChatService, ONNXChatService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configured Swagger to show the "Authorize" button for your long API key validation
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "ONNX API", 
        Description = "Deepseek R1 Distill - Qwen 14B", 
        Version = "v1" });

    //deepseek-r1-distill-qwen-14b
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme {
        Type = SecuritySchemeType.ApiKey,
        Name = "X-API-Key",
        In = ParameterLocation.Header,
        Description = "API key required. e.g.: eyJhbGciOiJIUzI1N..."
    });

    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement {
        [new OpenApiSecuritySchemeReference("ApiKey", document)] = []
    });
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.UseWebSockets();



app.Map("/ws/chat", async context => {
    if (!context.WebSockets.IsWebSocketRequest) {
        context.Response.StatusCode = 400;
        return;
    }

    using var socket = await context.WebSockets.AcceptWebSocketAsync();

    var buffer = new byte[4096];
    var result = await socket.ReceiveAsync(buffer, context.RequestAborted);
    var prompt = Encoding.UTF8.GetString(buffer, 0, result.Count);

    var chatService = context.RequestServices
        .GetRequiredService<IChatService>();

    await chatService.ChatAsync(prompt, context.RequestAborted);

    await socket.CloseAsync(
        WebSocketCloseStatus.NormalClosure,
        "Done",
        context.RequestAborted);
}).AddEndpointFilter(async (context, next) => {
    var http = context.HttpContext;
    var config = http.RequestServices.GetRequiredService<IConfiguration>();

    var expectedApiKey = config["ApiKey"];
    var actualApiKey = http.Request.Headers["X-Api-Key"].ToString();

    if (actualApiKey != expectedApiKey)
        return Results.Unauthorized();

    return await next(context);
});

app.Run();
