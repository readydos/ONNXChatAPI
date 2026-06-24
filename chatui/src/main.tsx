import React from "react";
import ReactDOM from "react-dom/client";
import { BaseProvider, DarkTheme } from "baseui";
import { Client as Styletron } from "styletron-engine-atomic";
import { Provider as StyletronProvider } from "styletron-react";

import App from "./App";

const engine = new Styletron();

ReactDOM.createRoot(document.getElementById("root")!).render(
    <React.StrictMode>
        <StyletronProvider value={engine}>
            <BaseProvider theme={DarkTheme}>
                <App />
            </BaseProvider>
        </StyletronProvider>
    </React.StrictMode>
);