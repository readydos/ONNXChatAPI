using OnnxChatApi.Options;
using OnnxChatApi.Services;
using Microsoft.OpenApi;

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
        Description = "API key required. Default: SecurityPassword123!"
    });

    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement {
        [new OpenApiSecuritySchemeReference("ApiKey", document)] = []
    });
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
