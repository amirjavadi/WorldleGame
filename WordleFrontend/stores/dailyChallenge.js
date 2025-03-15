import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import axios from 'axios';

export const useDailyChallengeStore = defineStore('dailyChallenge', () => {
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

  async function fetchTodaysChallenge() {
    try {
      loading.value = true;
      error.value = null;
      const response = await axios.get('/api/daily-challenges/today');
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
      loading.value = true;
      error.value = null;
      const response = await axios.post('/api/daily-challenges/participate');
      participation.value = response.data;
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'خطا در شروع مشارکت';
      participation.value = null;
    } finally {
      loading.value = false;
    }
  }

  async function makeGuess(guess) {
    if (!participation.value) return;

    try {
      loading.value = true;
      error.value = null;
      const response = await axios.post(`/api/daily-challenges/guess/${participation.value.id}`, { guess });
      participation.value = response.data;
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'خطا در ثبت حدس';
    } finally {
      loading.value = false;
    }
  }

  async function fetchLeaderboard() {
    try {
      loading.value = true;
      error.value = null;
      const response = await axios.get('/api/daily-challenges/leaderboard');
      leaderboard.value = response.data;
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'خطا در دریافت جدول رتبه‌بندی';
      leaderboard.value = [];
    } finally {
      loading.value = false;
    }
  }

  async function checkParticipation() {
    try {
      loading.value = true;
      error.value = null;
      const response = await axios.get('/api/daily-challenges/participation');
      participation.value = response.data;
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'خطا در بررسی وضعیت مشارکت';
      participation.value = null;
    } finally {
      loading.value = false;
    }
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
    checkParticipation
  };
}); 