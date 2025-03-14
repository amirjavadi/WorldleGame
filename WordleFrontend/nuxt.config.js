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
    preference: 'light',
    fallback: 'light',
    dataValue: 'theme',
    storageKey: 'theme'
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
  ],
  app: {
    head: {
      htmlAttrs: {
        lang: 'fa'
      }
    }
  },
  tailwindcss: {
    darkMode: 'class',
    config: {
      content: [
        './components/**/*.{js,vue,ts}',
        './layouts/**/*.vue',
        './pages/**/*.vue',
        './plugins/**/*.{js,ts}',
        './nuxt.config.{js,ts}',
      ]
    }
  }
}) 