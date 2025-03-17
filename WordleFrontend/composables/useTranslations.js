import { ref, computed, watch } from 'vue'
import { useHead } from '#imports'

export const useTranslations = () => {
  // ایمن‌سازی دسترسی به localStorage
  const getStoredLocale = () => {
    if (process.client) {
      return localStorage.getItem('preferredLocale') || 'fa'
    }
    return 'fa'
  }

  const locale = ref(getStoredLocale())
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
      playAsGuest: 'ورود به عنوان مهمان',
      // Game related translations
      gameTitle: 'بازی وردل فارسی',
      guessWord: 'کلمه را حدس بزنید',
      submit: 'ارسال',
      enter: 'ارسال',
      backspace: 'پاک کردن',
      score: 'امتیاز',
      attempts: 'تلاش‌ها',
      gameOver: 'بازی تمام شد',
      youWon: 'شما برنده شدید!',
      youLost: 'شما باختید!',
      tryAgain: 'تلاش مجدد',
      leaderboard: 'جدول امتیازات',
      rank: 'رتبه',
      player: 'بازیکن',
      guestPlayer: 'بازیکن مهمان',
      invalidWord: 'کلمه نامعتبر',
      wordTooShort: 'کلمه باید ۵ حرفی باشد',
      statistics: 'آمار بازی',
      totalGames: 'کل بازی‌ها',
      gamesPlayed: 'بازی‌های انجام شده',
      winRate: 'درصد برد',
      currentStreak: 'برد متوالی فعلی',
      maxStreak: 'بیشترین برد متوالی',
      bestStreak: 'بهترین رکورد',
      averageAttempts: 'میانگین تلاش‌ها',
      playAgain: 'بازی مجدد',
      easy: 'آسان',
      medium: 'متوسط',
      hard: 'سخت',
      difficulty: 'سختی بازی',
      noHelpLeft: 'راهنمایی باقی نمانده است',
      noNewHelpAvailable: 'راهنمایی جدیدی در دسترس نیست',
      logout: 'خروج',
      profile: 'پروفایل',
      userInfo: 'اطلاعات کاربری',
      close: 'بستن',
      noGamesYet: 'هنوز بازی‌ای انجام نشده است',
      won: 'برنده',
      lost: 'باخته',
      date: 'تاریخ',
      word: 'کلمه',
      result: 'نتیجه',
      playerProfile: 'پروفایل بازیکن',
      totalScore: 'امتیاز کل',
      guessDistribution: 'توزیع حدس‌ها',
      dailyChallenge: 'چالش روزانه',
      dailyChallengeDescription: 'هر روز یک کلمه جدید',
      timeUntilNextChallenge: 'تا چالش بعدی',
      hours: 'ساعت',
      minutes: 'دقیقه',
      seconds: 'ثانیه',
      weeklyStats: 'آمار هفتگی',
      monthlyStats: 'آمار ماهانه',
      lastUpdate: 'آخرین به‌روزرسانی',
      // Guest modal translations
      unlockPremium: 'ویژگی‌های حساب کاربری',
      guestModalDescription: 'با ایجاد حساب کاربری به امکانات بیشتری دسترسی خواهید داشت',
      progressTracking: 'پیگیری پیشرفت',
      achievements: 'دستاوردها',
      multiplayerMode: 'حالت چند نفره',
      cloudSync: 'همگام‌سازی ابری',
      continueFree: 'ادامه به عنوان مهمان',
      // Access denied translations
      guestAccessDenied: 'دسترسی محدود شده',
      pleaseLoginToAccessDaily: 'برای دسترسی به چالش روزانه لطفا وارد شوید یا ثبت‌نام کنید',
      pleaseLoginFirst: 'لطفا ابتدا وارد شوید'
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
      playAsGuest: 'Play as Guest',
      // Game related translations
      gameTitle: 'Persian Wordle Game',
      guessWord: 'Guess the Word',
      submit: 'Submit',
      enter: 'Enter',
      backspace: 'Backspace',
      score: 'Score',
      attempts: 'Attempts',
      gameOver: 'Game Over',
      youWon: 'You Won!',
      youLost: 'You Lost!',
      tryAgain: 'Try Again',
      leaderboard: 'Leaderboard',
      rank: 'Rank',
      player: 'Player',
      guestPlayer: 'Guest Player',
      invalidWord: 'Invalid Word',
      wordTooShort: 'Word must be 5 letters',
      statistics: 'Statistics',
      totalGames: 'Total Games',
      gamesPlayed: 'Games Played',
      winRate: 'Win Rate',
      currentStreak: 'Current Streak',
      maxStreak: 'Max Streak',
      bestStreak: 'Best Streak',
      averageAttempts: 'Average Attempts',
      playAgain: 'Play Again',
      easy: 'Easy',
      medium: 'Medium',
      hard: 'Hard',
      difficulty: 'Difficulty',
      noHelpLeft: 'No help remaining',
      noNewHelpAvailable: 'No new help available',
      logout: 'Logout',
      profile: 'Profile',
      userInfo: 'User Information',
      close: 'Close',
      noGamesYet: 'No games played yet',
      won: 'Won',
      lost: 'Lost',
      date: 'Date',
      word: 'Word',
      result: 'Result',
      playerProfile: 'Player Profile',
      totalScore: 'Total Score',
      guessDistribution: 'Guess Distribution',
      dailyChallenge: 'Daily Challenge',
      dailyChallengeDescription: 'New word every day',
      timeUntilNextChallenge: 'Until next challenge',
      hours: 'hours',
      minutes: 'minutes',
      seconds: 'seconds',
      weeklyStats: 'Weekly Stats',
      monthlyStats: 'Monthly Stats',
      lastUpdate: 'Last Update',
      // Guest modal translations
      unlockPremium: 'Account Features',
      guestModalDescription: 'Create an account to unlock more features',
      progressTracking: 'Progress Tracking',
      achievements: 'Achievements',
      multiplayerMode: 'Multiplayer Mode',
      cloudSync: 'Cloud Sync',
      continueFree: 'Continue as Guest',
      // Access denied translations
      guestAccessDenied: 'Access Restricted',
      pleaseLoginToAccessDaily: 'Please login or register to access daily challenge',
      pleaseLoginFirst: 'Please login first'
    }
  }

  const t = (key) => {
    return translations[locale.value][key] || key
  }

  const setLocale = (newLocale) => {
    locale.value = newLocale
    if (process.client) {
      localStorage.setItem('preferredLocale', newLocale)
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