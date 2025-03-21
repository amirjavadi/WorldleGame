import { createI18n } from 'vue-i18n'

export default defineNuxtPlugin(({ vueApp }) => {
  const i18n = createI18n({
    legacy: false,
    globalInjection: true,
    locale: 'fa',
    messages: {
      fa: {
        welcome: 'خوش آمدید',
        login: 'ورود',
        register: 'ثبت نام',
        username: 'نام کاربری',
        password: 'رمز عبور',
        confirmPassword: 'تکرار رمز عبور',
        email: 'ایمیل',
        loading: 'در حال بارگذاری...',
        playAsGuest: 'بازی به عنوان مهمان',
        noAccount: 'حساب کاربری ندارید؟',
        haveAccount: 'حساب کاربری دارید؟',
        passwordsDoNotMatch: 'رمزهای عبور مطابقت ندارند',
        invalidEmail: 'ایمیل نامعتبر است',
        // Game related translations
        gameTitle: 'بازی وردل فارسی',
        guessWord: 'حدس کلمه',
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
        logout: 'خروج'
      },
      en: {
        welcome: 'Welcome',
        login: 'Login',
        register: 'Register',
        username: 'Username',
        password: 'Password',
        confirmPassword: 'Confirm Password',
        email: 'Email',
        loading: 'Loading...',
        playAsGuest: 'Play as Guest',
        noAccount: "Don't have an account?",
        haveAccount: 'Already have an account?',
        passwordsDoNotMatch: 'Passwords do not match',
        invalidEmail: 'Invalid email address',
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
        logout: 'Logout'
      }
    }
  })

  vueApp.use(i18n)
}) 