import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vuetify from 'vite-plugin-vuetify'
// ✅ ADD THIS
import { fileURLToPath, URL } from 'node:url'

export default defineConfig({
  plugins: [vue(),vuetify({
autoImport:true
})],

  // ✅ ADD THIS BLOCK
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  }

})
