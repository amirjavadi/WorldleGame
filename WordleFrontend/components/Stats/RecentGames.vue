<template>
  <div>
    <h3 class="text-lg font-medium text-gray-900 dark:text-white mb-4">بازی‌های اخیر</h3>
    <div class="bg-white dark:bg-gray-800 shadow overflow-hidden rounded-lg">
      <ul class="divide-y divide-gray-200 dark:divide-gray-700">
        <li v-for="game in stats.lastGames" :key="game.id" class="px-6 py-4">
          <div class="flex items-center justify-between">
            <div>
              <div class="text-sm font-medium text-gray-900 dark:text-white">
                {{ game.word }}
              </div>
              <div class="text-sm text-gray-500 dark:text-gray-400">
                {{ new Date(game.date).toLocaleDateString('fa-IR') }}
              </div>
            </div>
            <div class="flex items-center">
              <span
                class="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium"
                :class="game.won ? 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200' : 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-200'"
              >
                {{ game.won ? 'برد' : 'باخت' }}
              </span>
              <span class="mr-4 text-sm text-gray-500 dark:text-gray-400">
                {{ game.guesses.length }} حدس
              </span>
            </div>
          </div>
          <div class="mt-2 flex space-x-1 space-x-reverse">
            <div
              v-for="(result, index) in game.guesses"
              :key="index"
              class="w-8 h-8 flex items-center justify-center rounded"
              :class="{
                'bg-green-500 dark:bg-green-600': result === 'correct',
                'bg-yellow-500 dark:bg-yellow-600': result === 'present',
                'bg-gray-300 dark:bg-gray-600': result === 'absent'
              }"
            ></div>
          </div>
        </li>
      </ul>
    </div>
  </div>
</template>

<script setup>
import { useStatsStore } from '@/stores/stats';

const store = useStatsStore();
const { stats } = store;
</script> 