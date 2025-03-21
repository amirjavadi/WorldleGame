<template>
  <div v-if="isOpen" class="fixed inset-0 z-50 flex items-center justify-center">
    <!-- Backdrop -->
    <div class="absolute inset-0 bg-black bg-opacity-50 transition-opacity" @click="$emit('close')"></div>

    <!-- Modal -->
    <div class="relative bg-white dark:bg-gray-800 rounded-lg shadow-xl w-full max-w-2xl mx-4 transform transition-all">
      <!-- Header -->
      <div class="flex items-center justify-between p-4 border-b dark:border-gray-700">
        <h2 class="text-xl font-semibold text-gray-900 dark:text-white">
          {{ t('dailyChallengeLeaderboardTitle') }}
        </h2>
        <button @click="$emit('close')" class="text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-200">
          <i class="fas fa-times"></i>
        </button>
      </div>

      <!-- Content -->
      <div class="p-4">
        <!-- Loading State -->
        <div v-if="loading" class="flex items-center justify-center py-8">
          <div class="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-500"></div>
        </div>

        <!-- Error State -->
        <div v-else-if="error" class="text-center py-8 text-red-500 dark:text-red-400">
          {{ t('dailyChallengeLeaderboardError') }}
        </div>

        <!-- Empty State -->
        <div v-else-if="!leaderboard.length" class="text-center py-8 text-gray-500 dark:text-gray-400">
          {{ t('dailyChallengeLeaderboardEmpty') }}
        </div>

        <!-- Leaderboard Table -->
        <div v-else class="space-y-2">
          <div v-for="(player, index) in leaderboard" :key="index"
            class="flex items-center justify-between p-3 rounded-lg transition-colors"
            :class="[
              index === 0 ? 'bg-yellow-50 dark:bg-yellow-900/20' :
              index === 1 ? 'bg-gray-50 dark:bg-gray-700/50' :
              index === 2 ? 'bg-amber-50 dark:bg-amber-900/20' :
              'hover:bg-gray-50 dark:hover:bg-gray-700/50'
            ]"
          >
            <!-- Rank -->
            <div class="flex items-center gap-3">
              <span class="w-8 text-center font-semibold"
                :class="[
                  index === 0 ? 'text-yellow-600 dark:text-yellow-400' :
                  index === 1 ? 'text-gray-600 dark:text-gray-400' :
                  index === 2 ? 'text-amber-600 dark:text-amber-400' :
                  'text-gray-500 dark:text-gray-400'
                ]"
              >
                {{ index + 1 }}
              </span>
              <div class="w-8 h-8 rounded-full bg-gray-200 dark:bg-gray-700 flex items-center justify-center">
                <span class="text-sm font-medium text-gray-600 dark:text-gray-300">
                  {{ player.username.charAt(0).toUpperCase() }}
                </span>
              </div>
            </div>

            <!-- Username -->
            <div class="flex-1 text-right mx-4">
              <span class="font-medium text-gray-900 dark:text-white">{{ player.username }}</span>
            </div>

            <!-- Score -->
            <div class="text-right">
              <span class="font-semibold text-gray-900 dark:text-white">{{ player.score }}</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { useTranslations } from '~/composables/useTranslations'

const { t } = useTranslations()

defineProps({
  isOpen: {
    type: Boolean,
    required: true
  },
  leaderboard: {
    type: Array,
    default: () => []
  },
  loading: {
    type: Boolean,
    default: false
  },
  error: {
    type: Boolean,
    default: false
  }
})

defineEmits(['close'])
</script> 