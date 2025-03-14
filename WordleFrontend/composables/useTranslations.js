import { ref, watch } from 'vue'
import { useHead } from '#imports'

export const useTranslations = () => {
  const locale = ref(process.client ? localStorage.getItem('locale') || 'fa' : 'fa')
  const dir = computed(() => locale.value === 'fa' ? 'rtl' : 'ltr')

  const translations = {
    fa: {
      welcome: 'خوش آمدید',
      login: 'ورود',
      register: 'ثبت نام',
      username: 'نام کاربری',
      password: 'رمز عبور',
      confirmPassword: 'تکرار رمز عبور',
      loading: 'در حال بارگذاری...',
      noAccount: 'حساب کاربری ندارید؟',
      haveAccount: 'حساب کاربری دارید؟',
      passwordsDoNotMatch: 'رمز عبور و تکرار آن مطابقت ندارند',
      guest: 'مهمان',
      email: 'ایمیل',
      invalidEmail: 'لطفا یک ایمیل معتبر وارد کنید',
      playAsGuest: 'ورود به عنوان مهمان'
    },
    en: {
      welcome: 'Welcome',
      login: 'Login',
      register: 'Register',
      username: 'Username',
      password: 'Password',
      confirmPassword: 'Confirm Password',
      loading: 'Loading...',
      noAccount: "Don't have an account?",
      haveAccount: 'Already have an account?',
      passwordsDoNotMatch: 'Passwords do not match',
      guest: 'Guest',
      email: 'Email',
      invalidEmail: 'Please enter a valid email address',
      playAsGuest: 'Play as Guest'
    }
  }

  const t = (key) => {
    return translations[locale.value][key] || key
  }

  const setLocale = (newLocale) => {
    locale.value = newLocale
    if (process.client) {
      localStorage.setItem('locale', newLocale)
    }
  }

  // Update document direction when locale changes
  watch(locale, (newLocale) => {
    useHead({
      htmlAttrs: {
        dir: newLocale === 'fa' ? 'rtl' : 'ltr',
        lang: newLocale
      }
    })
  }, { immediate: true })

  return {
    t,
    locale,
    setLocale,
    dir
  }
}

export default useTranslations 