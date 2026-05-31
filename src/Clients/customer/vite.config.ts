import { defineConfig } from "vite";
import tailwindcss from "@tailwindcss/vite";
import react from "@vitejs/plugin-react";
import p from "path";

// https://vite.dev/config/
export default defineConfig({
  plugins: [react(), tailwindcss()],
  resolve: {
    alias: {
      "@src": p.resolve(__dirname, "src"),
      "@assets": p.resolve(__dirname, "src/assets"),
      "@components": p.resolve(__dirname, "src/components"),
      "@features": p.resolve(__dirname, "src/features"),
      "@pages": p.resolve(__dirname, "src/pages"),
    },
  },
});
