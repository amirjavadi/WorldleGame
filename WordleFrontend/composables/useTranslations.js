import { ref } from 'vue'

const translations = {
  fa: {
    welcome: 'خوش آمدید',
    login: 'ورود',
    register: 'ثبت نام',
    username: 'نام کاربری',
    password: 'رمز عبور',
    confirmPassword: 'تکرار رمز عبور',
    loading: 'در حال بارگذاری...',
    noAccount: 'حساب کاربری ندارید؟ ثبت نام کنید',
    haveAccount: 'حساب کاربری دارید؟ وارد شوید',
    passwordsDoNotMatch: 'رمزهای عبور مطابقت ندارند',
    gameTitle: 'وردل فارسی',
    guessWord: 'کلمه را حدس بزنید',
    submit: 'ارسال',
    correct: 'درست',
    wrong: 'نادرست',
    wrongPosition: 'موقعیت نادرست',
    notInWord: 'در کلمه نیست',
    gameOver: 'بازی تمام شد',
    playAgain: 'بازی مجدد',
    win: 'تبریک! شما برنده شدید!',
    lose: 'متأسفانه باختید!',
    correctWord: 'کلمه صحیح',
    statistics: 'آمار',
    gamesPlayed: 'تعداد بازی‌ها',
    winRate: 'درصد برد',
    currentStreak: 'برد متوالی',
    bestStreak: 'بهترین برد متوالی',
    share: 'اشتراک',
    copy: 'کپی',
    copied: 'کپی شد!',
    language: 'زبان',
    theme: 'تم',
    light: 'روشن',
    dark: 'تاریک',
    settings: 'تنظیمات',
    logout: 'خروج',
    easy: 'آسان',
    medium: 'متوسط',
    hard: 'سخت',
    score: 'امتیاز',
    help: 'کمک',
    helpCount: 'تعداد کمک‌ها',
    helpUsed: 'یک حرف درست نشان داده شد',
    noHelpLeft: 'کمکی باقی نمانده است',
    rowIsFull: 'سطر فعلی پر است',
    noNewHelpAvailable: 'حرف جدیدی برای نمایش وجود ندارد',
    ok: 'تایید'
  },
  en: {
    welcome: 'Welcome',
    login: 'Login',
    register: 'Register',
    username: 'Username',
    password: 'Password',
    confirmPassword: 'Confirm Password',
    loading: 'Loading...',
    noAccount: 'No account? Register',
    haveAccount: 'Have an account? Login',
    passwordsDoNotMatch: 'Passwords do not match',
    gameTitle: 'Persian Wordle',
    guessWord: 'Guess the word',
    submit: 'Submit',
    correct: 'Correct',
    wrong: 'Wrong',
    wrongPosition: 'Wrong position',
    notInWord: 'Not in word',
    gameOver: 'Game Over',
    playAgain: 'Play Again',
    win: 'Congratulations! You won!',
    lose: 'Sorry, you lost!',
    correctWord: 'Correct word',
    statistics: 'Statistics',
    gamesPlayed: 'Games played',
    winRate: 'Win rate',
    currentStreak: 'Current streak',
    bestStreak: 'Best streak',
    share: 'Share',
    copy: 'Copy',
    copied: 'Copied!',
    language: 'Language',
    theme: 'Theme',
    light: 'Light',
    dark: 'Dark',
    settings: 'Settings',
    logout: 'Logout',
    easy: 'Easy',
    medium: 'Medium',
    hard: 'Hard',
    score: 'Score',
    help: 'Help',
    helpCount: 'Help Count',
    helpUsed: 'One correct letter revealed',
    noHelpLeft: 'No help left',
    rowIsFull: 'Current row is full',
    noNewHelpAvailable: 'No new letter available to reveal',
    ok: 'OK'
  }
}

export function useTranslations() {
  const locale = ref('fa')

  const t = (key) => {
    return translations[locale.value][key] || key
  }

  const setLocale = (newLocale) => {
    if (translations[newLocale]) {
      locale.value = newLocale
    }
  }

  return {
    t,
    setLocale,
    locale
  }
} 