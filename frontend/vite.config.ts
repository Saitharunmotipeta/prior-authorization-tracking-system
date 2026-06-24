import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// ✅ ADD THIS
import { fileURLToPath, URL } from 'node:url'

export default defineConfig({
  plugins: [vue()],

  // ✅ ADD THIS BLOCK
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  }
})
