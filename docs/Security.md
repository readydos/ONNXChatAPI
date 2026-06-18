# Security

The sample project intentionally exposes a public API endpoint.

For production deployments, implement:

## Authentication

- JWT Bearer Authentication
- OAuth2
- API Keys

## Network Security

- HTTPS
- Reverse Proxy
- WAF

## Rate Limiting

Example protections:

- Per-IP limits
- Request quotas
- Burst protection

## Input Validation

Validate:

- Request length
- Prompt size
- Character limits

## Logging

Recommended:

- Structured logging
- Request tracing
- Error monitoring

## CORS

Restrict allowed origins.

Example:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("Default", policy =>
    {
        policy.WithOrigins("https://myfrontend.com")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
```

## Prompt Safety

Protect against:

- Prompt injection
- Data exfiltration
- Jailbreak attempts
- Excessive token generation
