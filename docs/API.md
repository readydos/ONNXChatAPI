# API Documentation

## Endpoint

### POST /api/chat

Generates a response from the hosted ONNX model.

## Request

```json
{
  "message": "What is ONNX Runtime GenAI?"
}
```

## Response

```json
{
  "reply": "ONNX Runtime GenAI is..."
}
```

## Response Codes

| Status | Description |
|----------|----------|
| 200 | Successful generation |
| 400 | Invalid request |
| 500 | Internal server error |

## Swagger

Swagger UI is available at:

```text
/swagger
```
