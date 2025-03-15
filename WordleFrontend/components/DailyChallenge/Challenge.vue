<template>
  <div class="space-y-6">
    <!-- Challenge Header -->
    <div class="bg-white dark:bg-gray-800 shadow rounded-lg p-6">
      <div class="flex items-center justify-between">
        <h2 class="text-2xl font-bold text-gray-900 dark:text-white">چالش روزانه</h2>
        <div class="text-sm text-gray-500 dark:text-gray-400">
          <span>زمان باقی‌مانده: </span>
          <span class="font-medium">{{ formatTime(remainingTime) }}</span>
        </div>
      </div>
      
      <div class="mt-4">
        <p class="text-gray-600 dark:text-gray-300">
          {{ challenge?.description || 'توضیحات چالش در دسترس نیست.' }}
        </p>
        <div class="mt-2 flex items-center text-sm text-gray-500 dark:text-gray-400">
          <span>سطح دشواری: </span>
          <div class="mr-2 flex">
            <template v-for="i in 5" :key="i">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                class="h-5 w-5"
                :class="i <= (challenge?.difficulty || 0) ? 'text-yellow-400' : 'text-gray-300 dark:text-gray-600'"
                viewBox="0 0 20 20"
                fill="currentColor"
              >
                <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z" />
              </svg>
            </template>
          </div>
        </div>
      </div>

      <div class="mt-6">
        <button
          v-if="canParticipate"
          @click="startParticipation"
          :disabled="loading"
          class="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:opacity-50 disabled:cursor-not-allowed"
        >
          شروع چالش
        </button>
        <div v-else-if="isParticipating" class="text-sm text-gray-500 dark:text-gray-400">
          شما در حال شرکت در این چالش هستید.
        </div>
        <div v-else class="text-sm text-gray-500 dark:text-gray-400">
          شما قبلاً در این چالش شرکت کرده‌اید.
        </div>
      </div>
    </div>

    <!-- Game Board -->
    <div v-if="isParticipating" class="bg-white dark:bg-gray-800 shadow rounded-lg p-6">
      <GameBoard
        :word="participation?.word || ''"
        :guesses="participation?.guesses || []"
        :max-attempts="6"
        @make-guess="makeGuess"
      />
    </div>

    <!-- Challenge Leaderboard -->
    <div class="bg-white dark:bg-gray-800 shadow rounded-lg p-6">
      <h3 class="text-lg font-medium text-gray-900 dark:text-white mb-4">برترین‌های امروز</h3>
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
            <tr v-for="(entry, index) in leaderboard" :key="entry.userId" :class="{'bg-indigo-50 dark:bg-indigo-900/20': entry.isCurrentUser}">
              <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-100">{{ index + 1 }}</td>
              <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-100">
                <div class="flex items-center">
                  <span :class="{'font-semibold': entry.isCurrentUser}">{{ entry.username }}</span>
                  <span v-if="entry.isCurrentUser" class="mr-2 px-2 py-1 text-xs font-medium bg-indigo-100 dark:bg-indigo-800 text-indigo-700 dark:text-indigo-200 rounded-full">شما</span>
                </div>
              </td>
              <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-100">{{ entry.score }}</td>
              <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-100">{{ entry.guesses }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
import { onMounted, onUnmounted } from 'vue';
import { useDailyChallengeStore } from '@/stores/dailyChallenge';
import GameBoard from '@/components/GameBoard.vue';

const store = useDailyChallengeStore();

const {
  challenge,
  participation,
  loading,
  leaderboard,
  isParticipating,
  canParticipate,
  remainingTime
} = store;

let timer;

onMounted(async () => {
  await store.fetchTodaysChallenge();
  await store.checkParticipation();
  await store.fetchLeaderboard();
  
  // Update remaining time every second
  timer = setInterval(() => {
    if (remainingTime.value <= 0) {
      clearInterval(timer);
      store.fetchTodaysChallenge();
    }
  }, 1000);
});

onUnmounted(() => {
  if (timer) clearInterval(timer);
});

function formatTime(ms) {
  const hours = Math.floor(ms / (1000 * 60 * 60));
  const minutes = Math.floor((ms % (1000 * 60 * 60)) / (1000 * 60));
  const seconds = Math.floor((ms % (1000 * 60)) / 1000);
  
  return `${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
}

async function startParticipation() {
  await store.startParticipation();
}

async function makeGuess(guess) {
  await store.makeGuess(guess);
  await store.fetchLeaderboard();
}
</script> 