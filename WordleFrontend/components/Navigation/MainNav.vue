<template>
  <header class="bg-white/80 dark:bg-gray-800/80 backdrop-blur-sm shadow-lg">
    <div class="max-w-7xl mx-auto py-6 px-4 sm:px-6 lg:px-8">
      <div class="flex justify-between items-center">
        <NuxtLink to="/" class="text-3xl font-bold text-gray-900 dark:text-white font-display">
          {{ t('gameTitle') }}
        </NuxtLink>

        <div class="flex items-center space-x-4 rtl:space-x-reverse">
          <select
            v-model="difficulty"
            class="bg-white/80 dark:bg-gray-700/80 backdrop-blur-sm border border-gray-300 dark:border-gray-600 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 transition-all duration-200"
            @change="resetGame"
          >
            <option value="easy">{{ t('easy') }}</option>
            <option value="medium">{{ t('medium') }}</option>
            <option value="hard">{{ t('hard') }}</option>
          </select>

          <button
            @click="toggleLocale"
            class="flex items-center space-x-1 text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white transition-colors duration-200"
            :title="t('changeLanguage')"
          >
            <i class="fas fa-language text-xl"></i>
          </button>

          <button
            @click="themeStore.toggleTheme"
            class="flex items-center space-x-1 text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white transition-colors duration-200"
            :title="themeStore.isDark ? t('lightMode') : t('darkMode')"
          >
            <i :class="['fas', themeStore.isDark ? 'fa-sun' : 'fa-moon', 'text-xl']"></i>
          </button>

          <button
            @click="toggleSound"
            class="p-2 rounded-lg hover:bg-gray-200 dark:hover:bg-gray-700 transition-colors duration-200"
            :title="isSoundEnabled ? t('muteSound') : t('unmuteSound')"
          >
            <i :class="['fas', isSoundEnabled ? 'fa-volume-up' : 'fa-volume-mute', 'text-xl']"></i>
          </button>

          <ClientOnly>
            <template v-if="authStore.isLoggedIn">
              <NuxtLink
                to="/stats"
                class="flex items-center space-x-1 text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white transition-colors duration-200"
                :title="t('statistics')"
              >
                <i class="fas fa-chart-bar text-xl"></i>
              </NuxtLink>

              <button
                v-if="authStore.isLoggedIn && !authStore.isGuest"
                @click="openLeaderboardModal"
                class="flex items-center space-x-1 text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white transition-colors duration-200"
                :title="t('leaderboard')"
              >
                <i class="fas fa-trophy text-xl"></i>
              </button>

              <div class="flex items-center space-x-2 bg-gradient-to-r from-indigo-100 to-purple-100 dark:from-indigo-900 dark:to-purple-900 px-4 py-2 rounded-lg shadow-sm">
                <i class="fas fa-trophy text-yellow-500 text-xl"></i>
                <span class="text-lg font-bold text-indigo-700 dark:text-indigo-300">{{ score }}</span>
                <span class="text-sm font-medium text-indigo-600 dark:text-indigo-400">{{ t('score') }}</span>
              </div>

              <div class="flex items-center space-x-2 rtl:space-x-reverse">
                <button
                  @click="openProfileModal"
                  class="flex items-center space-x-2 rtl:space-x-reverse hover:text-indigo-600 dark:hover:text-indigo-400 transition-colors duration-200"
                >
                  <i class="fas fa-user-circle text-xl text-gray-600 dark:text-gray-300"></i>
                  <span class="text-gray-900 dark:text-white font-medium">{{ authStore.username }}</span>
                </button>
              </div>

              <button
                @click="authStore.logout"
                class="flex items-center space-x-1 text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white transition-colors duration-200"
                :title="t('logout')"
              >
                <i class="fas fa-sign-out-alt text-xl"></i>
              </button>
            </template>
            <template v-else>
              <NuxtLink
                to="/login"
                class="text-gray-500 dark:text-gray-300 hover:text-gray-700 dark:hover:text-gray-200 px-3 py-2 rounded-md text-sm font-medium"
              >
                {{ t('login') }}
              </NuxtLink>
              <NuxtLink
                to="/register"
                class="text-gray-500 dark:text-gray-300 hover:text-gray-700 dark:hover:text-gray-200 px-3 py-2 rounded-md text-sm font-medium"
              >
                {{ t('register') }}
              </NuxtLink>
            </template>
          </ClientOnly>
        </div>
      </div>
    </div>
  </header>
  <ProfileModal
    v-if="authStore.isLoggedIn && !authStore.isGuest"
    :is-open="isProfileModalOpen"
    @close="isProfileModalOpen = false"
  />
  <LeaderboardModal
    v-if="authStore.isLoggedIn && !authStore.isGuest"
    :is-open="isLeaderboardModalOpen"
    @close="isLeaderboardModalOpen = false"
  />
  <DailyModal
    v-if="showDailyModal"
    @close="showDailyModal = false"
  />
</template>

<script setup>
import { ref, watch } from 'vue'
import { useAuthStore } from '~/stores/auth'
import { useThemeStore } from '~/stores/theme'
import { useTranslations } from '~/composables/useTranslations'
import { useGameStore } from '~/stores/game'
import ProfileModal from '~/components/Profile/ProfileModal.vue'
import LeaderboardModal from '~/components/Leaderboard/LeaderboardModal.vue'
import DailyModal from '~/components/Daily/DailyModal.vue'

const authStore = useAuthStore()
const themeStore = useThemeStore()
const gameStore = useGameStore()
const { t, locale, setLocale } = useTranslations()

const difficulty = ref('medium')
const isSoundEnabled = ref(true)
const score = ref(0)
const isProfileModalOpen = ref(false)
const isLeaderboardModalOpen = ref(false)
const showDailyModal = ref(false)

const toggleLocale = () => {
  setLocale(locale.value === 'fa' ? 'en' : 'fa')
}

const toggleSound = () => {
  isSoundEnabled.value = !isSoundEnabled.value
}

const resetGame = () => {
  gameStore.resetGame(difficulty.value)
}

const openProfileModal = () => {
  if (!authStore.isLoggedIn || authStore.isGuest) {
    return;
  }
  isProfileModalOpen.value = true;
}

const openLeaderboardModal = () => {
  if (!authStore.isLoggedIn || authStore.isGuest) {
    return;
  }
  isLeaderboardModalOpen.value = true;
}

// Reset modals when user logs out
watch(() => authStore.isLoggedIn, (isLoggedIn) => {
  if (!isLoggedIn) {
    isProfileModalOpen.value = false
    isLeaderboardModalOpen.value = false
  }
})
</script> 