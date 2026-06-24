import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import mkcert from 'vite-plugin-mkcert'

export default defineConfig({
    plugins: [plugin(), mkcert()],
    server: {
        port: 55368
    }
})
