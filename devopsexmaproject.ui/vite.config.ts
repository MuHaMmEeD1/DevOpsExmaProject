import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [react()],
    server: {
        host: true,  
        port: 5173,  
        proxy: {
            '/api': {
                target: process.env.NODE_ENV === 'production'
                    ? 'http://apigateway:8080'
                    : 'http://localhost:5001',
                changeOrigin: true,
                rewrite: (path: string) => path.replace(/^\/api/, '')
            }
        }
    }
});
