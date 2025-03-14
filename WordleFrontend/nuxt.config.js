// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  devtools: { enabled: true },
  modules: [
    '@nuxtjs/tailwindcss',
    '@pinia/nuxt',
    '@nuxtjs/color-mode'
  ],
  colorMode: {
    classSuffix: '',
    preference: 'system',
    fallback: 'light'
  },
  runtimeConfig: {
    public: {
      apiBase: 'http://localhost:5275/api'
    }
  },
  compatibilityDate: '2025-03-13',
  css: [
    '@/assets/css/main.css',
    '@fortawesome/fontawesome-free/css/all.css'
  ]
}) 