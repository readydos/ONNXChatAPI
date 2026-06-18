# OnnxChatApi

ASP.NET Core Web API that hosts an ONNX GenAI model and exposes a public chat endpoint with Swagger.

## Architecture

```mermaid
flowchart LR
    Client[Public Client] -->|POST /api/chat| API[ASP.NET Core Web API]
    API --> Swagger[Swagger / Swashbuckle]
    API --> ChatController[ChatController]
    ChatController --> ChatService[OnnxChatService]
    ChatService --> Prompt[System Message + User Message]
    Prompt --> ORT[ONNX Runtime GenAI]
    ORT --> Model[(ONNX GenAI Model Folder)]
    ORT --> Response[Generated Text]
    Response --> API
