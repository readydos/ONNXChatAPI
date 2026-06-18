# Deployment

## Docker Example

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY publish/ .

EXPOSE 5000

ENTRYPOINT ["dotnet", "OnnxChatApi.dll"]
```

## Publish

```bash
dotnet publish -c Release -o publish
```

## Run Container

```bash
docker build -t onnx-chat-api .

docker run \
    -p 5000:5000 \
    -v ./models:/app/models \
    onnx-chat-api
```

## Kubernetes

Recommended additions:

- Readiness probe
- Liveness probe
- HPA
- Ingress
- TLS termination
