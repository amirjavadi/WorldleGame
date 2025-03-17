<template>
  <div class="space-y-6">
    <!-- Challenge Header -->
    <div class="bg-white dark:bg-gray-800 shadow rounded-lg p-6 transform transition-all duration-300 hover:shadow-lg">
      <div class="flex items-center justify-between">
        <div class="flex items-center space-x-2 rtl:space-x-reverse">
          <i class="fas fa-calendar-day text-2xl text-indigo-500 dark:text-indigo-400"></i>
          <h2 class="text-2xl font-bold text-gray-900 dark:text-white">چالش روزانه</h2>
        </div>
        <CountdownTimer :remaining-time="remainingTime" />
      </div>
      
      <div class="mt-6">
        <div class="bg-gradient-to-r from-indigo-50 to-purple-50 dark:from-indigo-900/20 dark:to-purple-900/20 rounded-lg p-4">
          <p class="text-gray-600 dark:text-gray-300">
            {{ challenge?.description || 'توضیحات چالش در دسترس نیست.' }}
          </p>
          <div class="mt-3 flex items-center text-sm text-gray-500 dark:text-gray-400">
            <span>سطح دشواری: </span>
            <div class="mr-2 flex">
              <template v-for="i in 5" :key="i">
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-5 w-5 transition-all duration-300"
                  :class="[
                    i <= (challenge?.difficulty || 0) ? 'text-yellow-400 animate-bounce' : 'text-gray-300 dark:text-gray-600',
                    {'delay-100': i === 1, 'delay-200': i === 2, 'delay-300': i === 3, 'delay-400': i === 4, 'delay-500': i === 5}
                  ]"
                  viewBox="0 0 20 20"
                  fill="currentColor"
                >
                  <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z" />
                </svg>
              </template>
            </div>
          </div>
        </div>
      </div>

      <div class="mt-6">
        <div v-if="error" class="mb-4 p-4 bg-red-100 dark:bg-red-900/20 text-red-700 dark:text-red-300 rounded-lg">
          {{ error }}
        </div>

        <button
          v-if="canParticipate"
          @click="startParticipation"
          :disabled="loading"
          class="w-full flex justify-center items-center py-3 px-4 border border-transparent rounded-lg text-sm font-medium text-white bg-gradient-to-r from-indigo-600 to-purple-600 hover:from-indigo-700 hover:to-purple-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:opacity-50 disabled:cursor-not-allowed transform transition-all duration-300 hover:scale-[1.02] hover:shadow-lg"
        >
          <i class="fas fa-play-circle mr-2"></i>
          شروع چالش
          <span v-if="loading" class="mr-2">
            <i class="fas fa-circle-notch animate-spin"></i>
          </span>
        </button>
        <div v-else-if="isParticipating" class="text-sm bg-green-100 dark:bg-green-900/20 text-green-700 dark:text-green-300 p-4 rounded-lg">
          <i class="fas fa-gamepad mr-2"></i>
          شما در حال شرکت در این چالش هستید.
        </div>
        <div v-else class="text-sm bg-yellow-100 dark:bg-yellow-900/20 text-yellow-700 dark:text-yellow-300 p-4 rounded-lg">
          <i class="fas fa-check-circle mr-2"></i>
          شما قبلاً در این چالش شرکت کرده‌اید.
        </div>
      </div>
    </div>

    <!-- Game Board -->
    <div v-if="isParticipating" class="bg-white dark:bg-gray-800 shadow rounded-lg p-6 transform transition-all duration-300 hover:shadow-lg">
      <GameBoard
        :word="participation?.word || ''"
        :guesses="participation?.guesses || []"
        :max-attempts="6"
        :show-score="true"
        :score="participation?.score || 0"
        :personal-best="personalBest"
        :is-interactive="!participation?.completed"
        @make-guess="makeGuess"
      />
    </div>

    <!-- Challenge Leaderboard -->
    <div class="bg-white dark:bg-gray-800 shadow rounded-lg p-6 transform transition-all duration-300 hover:shadow-lg">
      <div class="flex items-center justify-between mb-6">
        <div class="flex items-center space-x-2 rtl:space-x-reverse">
          <i class="fas fa-trophy text-2xl text-yellow-500"></i>
          <h3 class="text-lg font-medium text-gray-900 dark:text-white">برترین‌های امروز</h3>
        </div>
        <button
          @click="fetchLeaderboard"
          class="text-indigo-600 dark:text-indigo-400 hover:text-indigo-700 dark:hover:text-indigo-300 transition-colors duration-300"
          :disabled="loading"
        >
          <i class="fas fa-sync-alt" :class="{ 'animate-spin': loading }"></i>
        </button>
      </div>
      
      <div class="overflow-x-auto">
        <table class="min-w-full divide-y divide-gray-200 dark:divide-gray-700">
          <thead>
            <tr>
              <th class="px-6 py-3 bg-gray-50 dark:bg-gray-800 text-right text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">رتبه</th>
              <th class="px-6 py-3 bg-gray-50 dark:bg-gray-800 text-right text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">نام کاربری</th>
              <th class="px-6 py-3 bg-gray-50 dark:bg-gray-800 text-right text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">امتیاز</th>
              <th class="px-6 py-3 bg-gray-50 dark:bg-gray-800 text-right text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">تعداد حدس</th>
            </tr>
          </thead>
          <tbody class="bg-white dark:bg-gray-900 divide-y divide-gray-200 dark:divide-gray-700">
            <tr v-for="(entry, index) in leaderboard" :key="entry.userId" 
                :class="{'bg-indigo-50 dark:bg-indigo-900/20': entry.isCurrentUser}"
                class="transform transition-all duration-300 hover:bg-gray-50 dark:hover:bg-gray-800/50">
              <td class="px-6 py-4 whitespace-nowrap">
                <div class="flex items-center">
                  <div :class="[
                    'flex-shrink-0 h-8 w-8 rounded-full flex items-center justify-center text-sm font-bold',
                    index === 0 ? 'bg-yellow-100 dark:bg-yellow-900/20 text-yellow-700 dark:text-yellow-300' :
                    index === 1 ? 'bg-gray-100 dark:bg-gray-800 text-gray-700 dark:text-gray-300' :
                    index === 2 ? 'bg-amber-100 dark:bg-amber-900/20 text-amber-700 dark:text-amber-300' :
                    'bg-gray-50 dark:bg-gray-800/50 text-gray-600 dark:text-gray-400'
                  ]">
                    {{ index + 1 }}
                  </div>
                </div>
              </td>
              <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-100">
                <div class="flex items-center">
                  <span :class="{'font-semibold': entry.isCurrentUser}">{{ entry.username }}</span>
                  <span v-if="entry.isCurrentUser" class="mr-2 px-2 py-1 text-xs font-medium bg-indigo-100 dark:bg-indigo-800 text-indigo-700 dark:text-indigo-200 rounded-full animate-pulse">
                    شما
                  </span>
                </div>
              </td>
              <td class="px-6 py-4 whitespace-nowrap">
                <div class="text-sm font-medium text-gray-900 dark:text-gray-100">{{ entry.score }}</div>
                <div class="text-xs text-gray-500 dark:text-gray-400">امتیاز</div>
              </td>
              <td class="px-6 py-4 whitespace-nowrap">
                <div class="text-sm font-medium text-gray-900 dark:text-gray-100">{{ entry.guesses }}</div>
                <div class="text-xs text-gray-500 dark:text-gray-400">حدس</div>
              </td>
            </tr>
            <tr v-if="leaderboard.length === 0">
              <td colspan="4" class="px-6 py-8 text-center text-gray-500 dark:text-gray-400">
                <i class="fas fa-users-slash text-4xl mb-2"></i>
                <p>هنوز هیچ شرکت‌کننده‌ای در چالش امروز نیست.</p>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
import { onMounted, onUnmounted, ref } from 'vue';
import { useDailyChallengeStore } from '@/stores/dailyChallenge';
import { useAuthStore } from '@/stores/auth';
import GameBoard from '@/components/GameBoard.vue';
import CountdownTimer from './CountdownTimer.vue';

const store = useDailyChallengeStore();
const authStore = useAuthStore();
const personalBest = ref(null);

const {
  challenge,
  participation,
  loading,
  error,
  leaderboard,
  isParticipating,
  canParticipate,
  remainingTime
} = store;

let timer;

onMounted(async () => {
  if (!authStore.isLoggedIn) {
    store.setError('برای شرکت در چالش روزانه باید وارد حساب کاربری خود شوید.');
    return;
  }

  try {
    await store.fetchTodaysChallenge();
    await store.checkParticipation();
    await store.fetchLeaderboard();
    
    // Get personal best from localStorage
    const stats = JSON.parse(localStorage.getItem('dailyChallengeStats') || '{}');
    personalBest.value = stats.personalBest || null;
    
    // Update remaining time every second
    timer = setInterval(() => {
      if (remainingTime.value <= 0) {
        clearInterval(timer);
        store.fetchTodaysChallenge();
      }
    }, 1000);
  } catch (err) {
    store.setError('خطا در دریافت اطلاعات چالش روزانه. لطفاً دوباره تلاش کنید.');
  }
});

onUnmounted(() => {
  if (timer) clearInterval(timer);
});

async function startParticipation() {
  if (!authStore.isLoggedIn) {
    store.setError('برای شرکت در چالش روزانه باید وارد حساب کاربری خود شوید.');
    return;
  }

  try {
    await store.startParticipation();
  } catch (err) {
    store.setError('خطا در شروع چالش. لطفاً دوباره تلاش کنید.');
  }
}

async function makeGuess(guess) {
  if (!authStore.isLoggedIn) {
    store.setError('برای ثبت حدس باید وارد حساب کاربری خود شوید.');
    return;
  }

  try {
    await store.makeGuess(guess);
    await store.fetchLeaderboard();
    
    // Update personal best if needed
    if (participation.value?.completed && participation.value?.score) {
      const stats = JSON.parse(localStorage.getItem('dailyChallengeStats') || '{}');
      if (!stats.personalBest || participation.value.score > stats.personalBest) {
        stats.personalBest = participation.value.score;
        localStorage.setItem('dailyChallengeStats', JSON.stringify(stats));
        personalBest.value = stats.personalBest;
      }
    }
  } catch (err) {
    store.setError('خطا در ثبت حدس. لطفاً دوباره تلاش کنید.');
  }
}
</script>

<style scoped>
@keyframes bounce {
  0%, 100% {
    transform: translateY(0);
  }
  50% {
    transform: translateY(-5px);
  }
}

.animate-bounce {
  animation: bounce 2s infinite;
}

@keyframes pulse {
  0%, 100% {
    opacity: 1;
  }
  50% {
    opacity: 0.5;
  }
}

.animate-pulse {
  animation: pulse 2s cubic-bezier(0.4, 0, 0.6, 1) infinite;
}
</style> 