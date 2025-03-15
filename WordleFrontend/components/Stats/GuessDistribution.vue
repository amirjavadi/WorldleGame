<template>
  <div>
    <h3 class="text-lg font-medium text-gray-900 dark:text-white mb-4">توزیع حدس‌ها</h3>
    <div class="space-y-2">
      <div v-for="(count, guess) in distribution" :key="guess" class="flex items-center">
        <span class="w-4 text-sm text-gray-600 dark:text-gray-400">{{ guess }}</span>
        <div class="flex-1 mr-2">
          <div
            class="bg-indigo-100 dark:bg-indigo-900 text-indigo-800 dark:text-indigo-200 text-sm px-3 py-1 rounded"
            :style="{ width: `${(count / maxCount) * 100}%`, minWidth: count > 0 ? '2rem' : '0' }"
          >
            {{ count }}
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { useStatsStore } from '@/stores/stats';

const store = useStatsStore();
const { stats, maxGuessCount } = store;

const distribution = computed(() => stats.value.guessDistribution);
const maxCount = computed(() => maxGuessCount.value || 1);
</script> 