// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  devtools: { enabled: false },
  ssr: false,
  modules: [
    '@nuxtjs/tailwindcss',
    '@pinia/nuxt',
    '@nuxtjs/color-mode',
    'pinia-plugin-persistedstate/nuxt'
  ],
  plugins: ['~/plugins/vuetify.js'],
  colorMode: {
    classSuffix: '',
    preference: 'system',
    fallback: 'light',
    dataValue: 'theme',
    storageKey: 'nuxt-color-mode',
    classPrefix: '',
    componentName: 'ColorScheme',
    globalName: '__NUXT_COLOR_MODE__',
    storageMode: 'localStorage',
    classNames: {
      light: 'light',
      dark: 'dark'
    }
  },
  piniaPluginPersistedstate: {
    storage: 'localStorage',
    debug: true
  },
  runtimeConfig: {
    public: {
      apiBase: process.env.API_BASE_URL || 'http://localhost:5275/api'
    }
  },
  compatibilityDate: '2025-03-13',
  css: [
    '@fortawesome/fontawesome-free/css/all.css',
    '~/assets/css/main.css',
    '~/assets/css/theme.css',
    'vuetify/styles'
  ],
  app: {
    head: {
      htmlAttrs: {
        lang: 'fa',
        dir: 'rtl'
      },
      meta: [
        { charset: 'utf-8' },
        { name: 'viewport', content: 'width=device-width, initial-scale=1' }
      ]
    },
    baseURL: '/'
  },
  tailwindcss: {
    darkMode: 'class',
    config: {
      content: [
        './components/**/*.{js,vue}',
        './layouts/**/*.vue',
        './pages/**/*.vue',
        './plugins/**/*.js',
        './nuxt.config.js',
      ]
    }
  },
  build: {
    transpile: ['vuetify']
  },
  nitro: {
    compressPublicAssets: true
  },
  experimental: {
    payloadExtraction: false
  }
}) 