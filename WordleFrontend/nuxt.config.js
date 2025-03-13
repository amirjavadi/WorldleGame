// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  devtools: { enabled: true },
  modules: [
    '@nuxtjs/tailwindcss',
    '@pinia/nuxt',
    '@nuxtjs/i18n',
    '@nuxtjs/color-mode'
  ],
  i18n: {
    locales: ['fa', 'en'],
    defaultLocale: 'fa',
    vueI18n: {
      legacy: false,
      locale: 'fa',
      messages: {
        fa: {
          welcome: 'خوش آمدید',
          login: 'ورود',
          register: 'ثبت نام',
          play: 'بازی',
          history: 'تاریخچه',
          profile: 'پروفایل',
          username: 'نام کاربری',
          password: 'رمز عبور',
          email: 'ایمیل',
          confirmPassword: 'تکرار رمز عبور',
          loading: 'در حال بارگذاری...',
          noAccount: 'حساب کاربری ندارید؟',
          haveAccount: 'حساب کاربری دارید؟',
          loginError: 'خطا در ورود',
          registerError: 'خطا در ثبت نام',
          passwordsDoNotMatch: 'رمزهای عبور مطابقت ندارند'
        },
        en: {
          welcome: 'Welcome',
          login: 'Login',
          register: 'Register',
          play: 'Play',
          history: 'History',
          profile: 'Profile',
          username: 'Username',
          password: 'Password',
          email: 'Email',
          confirmPassword: 'Confirm Password',
          loading: 'Loading...',
          noAccount: 'Don\'t have an account?',
          haveAccount: 'Already have an account?',
          loginError: 'Login failed',
          registerError: 'Registration failed',
          passwordsDoNotMatch: 'Passwords do not match'
        }
      }
    }
  },
  colorMode: {
    classSuffix: '',
    preference: 'system',
    fallback: 'light'
  },
  runtimeConfig: {
    public: {
      apiBase: 'http://localhost:5275/api'
    }
  }
}) 