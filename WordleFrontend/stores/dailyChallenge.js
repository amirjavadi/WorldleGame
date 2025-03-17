import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import axios from 'axios';
import { useAuthStore } from './auth';

export const useDailyChallengeStore = defineStore('dailyChallenge', () => {
  const authStore = useAuthStore();
  const challenge = ref(null);
  const participation = ref(null);
  const loading = ref(false);
  const error = ref(null);
  const leaderboard = ref([]);

  const isParticipating = computed(() => !!participation.value);
  const canParticipate = computed(() => challenge.value && !participation.value);
  const remainingTime = computed(() => {
    if (!challenge.value) return 0;
    const endTime = new Date(challenge.value.endDate).getTime();
    const now = Date.now();
    return Math.max(0, endTime - now);
  });

  // Calculate score based on difficulty and attempts
  const calculateScore = (difficulty, attempts, maxAttempts) => {
    const baseScore = {
      1: 100, // آسان
      2: 200, // متوسط
      3: 300  // سخت
    }[difficulty] || 100;

    const attemptsMultiplier = Math.max(0, (maxAttempts - attempts + 1) / maxAttempts);
    const timeBonus = Math.floor((remainingTime.value / (24 * 60 * 60 * 1000)) * 50); // حداکثر 50 امتیاز برای زمان

    return Math.floor(baseScore * attemptsMultiplier + timeBonus);
  };

  async function fetchTodaysChallenge() {
    try {
      if (!authStore.isLoggedIn || authStore.isGuest) {
        error.value = 'برای مشاهده چالش روزانه باید وارد حساب کاربری خود شوید.';
        return;
      }

      loading.value = true;
      error.value = null;
      const response = await axios.get('/api/daily-challenges/today', {
        headers: {
          Authorization: `Bearer ${authStore.token}`
        }
      });
      challenge.value = response.data;
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'خطا در دریافت چالش روزانه';
      challenge.value = null;
    } finally {
      loading.value = false;
    }
  }

  async function startParticipation() {
    try {
      if (!authStore.isLoggedIn || authStore.isGuest) {
        error.value = 'برای شرکت در چالش روزانه باید وارد حساب کاربری خود شوید.';
        return;
      }

      loading.value = true;
      error.value = null;
      const response = await axios.post('/api/daily-challenges/participate', {}, {
        headers: {
          Authorization: `Bearer ${authStore.token}`
        }
      });
      participation.value = {
        ...response.data,
        score: 0,
        guesses: [],
        completed: false
      };
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'خطا در شروع مشارکت';
      participation.value = null;
    } finally {
      loading.value = false;
    }
  }

  async function makeGuess(guess) {
    if (!participation.value) return;
    if (!authStore.isLoggedIn || authStore.isGuest) {
      error.value = 'برای ثبت حدس باید وارد حساب کاربری خود شوید.';
      return;
    }

    try {
      loading.value = true;
      error.value = null;
      const response = await axios.post(`/api/daily-challenges/guess/${participation.value.id}`, 
        { guess },
        {
          headers: {
            Authorization: `Bearer ${authStore.token}`
          }
        }
      );
      
      const newParticipation = {
        ...response.data,
        guesses: [...(participation.value.guesses || []), {
          word: guess,
          result: response.data.lastGuessResult
        }]
      };

      // اگر حدس درست بود یا تعداد حدس‌ها به حداکثر رسیده
      if (response.data.isCorrect || newParticipation.guesses.length >= 6) {
        newParticipation.completed = true;
        newParticipation.score = calculateScore(
          challenge.value.difficulty,
          newParticipation.guesses.length,
          6
        );

        // ذخیره رکورد جدید اگر بهتر از رکورد قبلی است
        const stats = JSON.parse(localStorage.getItem('dailyChallengeStats') || '{}');
        if (!stats.personalBest || newParticipation.score > stats.personalBest) {
          stats.personalBest = newParticipation.score;
          localStorage.setItem('dailyChallengeStats', JSON.stringify(stats));
        }
      }

      participation.value = newParticipation;
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'خطا در ثبت حدس';
    } finally {
      loading.value = false;
    }
  }

  async function fetchLeaderboard() {
    try {
      if (!authStore.isLoggedIn || authStore.isGuest) {
        error.value = 'برای مشاهده جدول امتیازات باید وارد حساب کاربری خود شوید.';
        return;
      }

      loading.value = true;
      error.value = null;
      const response = await axios.get('/api/daily-challenges/leaderboard', {
        headers: {
          Authorization: `Bearer ${authStore.token}`
        }
      });
      leaderboard.value = response.data.map(entry => ({
        ...entry,
        isCurrentUser: entry.userId === authStore.userId
      }));
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'خطا در دریافت جدول رتبه‌بندی';
      leaderboard.value = [];
    } finally {
      loading.value = false;
    }
  }

  async function checkParticipation() {
    try {
      if (!authStore.isLoggedIn || authStore.isGuest) {
        error.value = 'برای بررسی وضعیت مشارکت باید وارد حساب کاربری خود شوید.';
        return;
      }

      loading.value = true;
      error.value = null;
      const response = await axios.get('/api/daily-challenges/participation', {
        headers: {
          Authorization: `Bearer ${authStore.token}`
        }
      });
      if (response.data) {
        participation.value = {
          ...response.data,
          score: response.data.score || 0,
          guesses: response.data.guesses || [],
          completed: response.data.completed || false
        };
      } else {
        participation.value = null;
      }
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'خطا در بررسی وضعیت مشارکت';
      participation.value = null;
    } finally {
      loading.value = false;
    }
  }

  function setError(message) {
    error.value = message;
  }

  return {
    challenge,
    participation,
    loading,
    error,
    leaderboard,
    isParticipating,
    canParticipate,
    remainingTime,
    fetchTodaysChallenge,
    startParticipation,
    makeGuess,
    fetchLeaderboard,
    checkParticipation,
    setError
  };
}); 