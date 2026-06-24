import { useState } from "react";
import { Button } from "baseui/button";
import { Input } from "baseui/input";
import { Card } from "baseui/card";
import { Block } from "baseui/block";

const API_URL = "https://localhost:7075/api/Chat";
const API_KEY = import.meta.env.VITE_API_KEY;

interface ChatMessage {
    role: "user" | "assistant";
    content: string;
}

interface ChatResponse {
    message?: string;
}

export default function App() {
    const [message, setMessage] = useState<string>("");
    const [loading, setLoading] = useState<boolean>(false);
    const [chat, setChat] = useState<ChatMessage[]>([]);

    const sendMessage = async (): Promise<void> => {
        if (!message.trim()) return;

        const userMessage = message;

        setChat((prev) => [
            ...prev,
            {
                role: "user",
                content: userMessage,
            },
        ]);

        setMessage("");
        setLoading(true);

        try {
            const response = await fetch(API_URL, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "X-API-KEY": API_KEY,
                },
                body: JSON.stringify({
                    message: userMessage,
                }),
            });

            if (!response.ok) {
                throw new Error(`HTTP ${response.status}`);
            }

            const data: ChatResponse = await response.json();

            setChat((prev) => [
                ...prev,
                {
                    role: "assistant",
                    content: data.message ?? "No response received",
                },
            ]);
        } catch (error) {
            setChat((prev) => [
                ...prev,
                {
                    role: "assistant",
                    content:
                        error instanceof Error
                            ? error.message
                            : "An unknown error occurred",
                },
            ]);
        } finally {
            setLoading(false);
        }
    };

    const handleKeyDown = (
        e: React.KeyboardEvent<HTMLInputElement | HTMLTextAreaElement>
    ): void => {
        if (e.key === "Enter" && !loading) {
            sendMessage();
        }
    };

    return (
        <Block
            display="flex"
            flexDirection="column"
            height="100vh"
            maxWidth="900px"
            margin="0 auto"
            padding="scale800"
        >
            <h1>Chat</h1>

            <Block
                flex="1"
                overflow="auto"
                display="flex"
                flexDirection="column"
                marginBottom="scale600"
            >
                {chat.map((msg, index) => (
                    <Card
                        key={index}
                        overrides={{
                            Root: {
                                style: {
                                    marginBottom: "12px",
                                    alignSelf:
                                        msg.role === "user"
                                            ? "flex-end"
                                            : "flex-start",
                                    maxWidth: "75%",
                                },
                            },
                        }}
                    >
                        <strong>{msg.role === "user" ? "You" : "Assistant"}</strong>

                        <div style={{ marginTop: 8 }}>{msg.content}</div>
                    </Card>
                ))}
            </Block>

            <Block display="flex">
                <Input
                    value={message}
                    onChange={(e) => setMessage(e.currentTarget.value)}
                    onKeyDown={handleKeyDown}
                    placeholder="Type a message..."
                    disabled={loading}
                    clearOnEscape
                />

                <Button
                    onClick={sendMessage}
                    isLoading={loading}
                    disabled={loading || !message.trim()}
                >
                    Send
                </Button>
            </Block>
        </Block>
    );
}