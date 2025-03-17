<template>
  <TransitionRoot appear :show="isOpen" as="template">
    <Dialog as="div" @close="closeModal" class="relative z-50">
      <TransitionChild
        as="template"
        enter="duration-300 ease-out"
        enter-from="opacity-0"
        enter-to="opacity-100"
        leave="duration-200 ease-in"
        leave-from="opacity-100"
        leave-to="opacity-0"
      >
        <div class="fixed inset-0 bg-black/30 backdrop-blur-sm" />
      </TransitionChild>

      <div class="fixed inset-0 overflow-y-auto">
        <div class="flex min-h-full items-center justify-center p-4">
          <TransitionChild
            as="template"
            enter="duration-300 ease-out"
            enter-from="opacity-0 scale-95"
            enter-to="opacity-100 scale-100"
            leave="duration-200 ease-in"
            leave-from="opacity-100 scale-100"
            leave-to="opacity-0 scale-95"
          >
            <DialogPanel :dir="dir" class="w-full max-w-3xl transform overflow-hidden rounded-3xl bg-gradient-to-br from-white to-gray-50 dark:from-gray-800 dark:to-gray-900 p-6 shadow-[0_0_50px_rgba(79,70,229,0.15)] dark:shadow-[0_0_50px_rgba(79,70,229,0.3)] transition-all">
              <div class="relative">
                <!-- Close Button -->
                <button
                  @click="closeModal"
                  :class="[
                    'absolute top-2 p-2 text-gray-400 hover:text-gray-500 dark:text-gray-500 dark:hover:text-gray-400 transition-colors duration-200 hover:rotate-90 transform',
                    dir === 'rtl' ? 'left-2' : 'right-2'
                  ]"
                >
                  <span class="sr-only">{{ t('close') }}</span>
                  <i class="fas fa-times text-xl"></i>
                </button>

                <!-- Header with Avatar -->
                <div class="text-center mb-4">
                  <div class="inline-block p-1.5 rounded-full bg-gradient-to-r from-indigo-500 to-purple-500 mb-2 animate-pulse-slow">
                    <div class="bg-white dark:bg-gray-800 rounded-full p-2">
                      <i class="fas fa-user-astronaut text-3xl bg-gradient-to-r from-indigo-500 to-purple-500 bg-clip-text text-transparent"></i>
                    </div>
                  </div>
                  <DialogTitle as="h3" class="text-2xl font-bold bg-gradient-to-r from-indigo-500 to-purple-500 bg-clip-text text-transparent">
                    {{ authStore.username }}
                  </DialogTitle>
                  <p class="mt-1 text-sm text-gray-500 dark:text-gray-400">{{ t('playerProfile') }}</p>
                </div>

                <!-- Content -->
                <div class="space-y-4">
                  <!-- Stats Overview -->
                  <div class="grid grid-cols-2 md:grid-cols-4 gap-3 mb-4">
                    <div class="bg-gradient-to-br from-indigo-500/10 to-purple-500/10 dark:from-indigo-500/20 dark:to-purple-500/20 rounded-xl p-3 transform hover:scale-105 transition-transform duration-200">
                      <div class="text-center">
                        <i class="fas fa-trophy text-xl text-yellow-500 mb-1"></i>
                        <h5 class="text-xs font-medium text-gray-500 dark:text-gray-400">{{ t('totalScore') }}</h5>
                        <p class="text-xl font-bold text-gray-900 dark:text-white">{{ userStats.score || 0 }}</p>
                      </div>
                    </div>
                    <div class="bg-gradient-to-br from-indigo-500/10 to-purple-500/10 dark:from-indigo-500/20 dark:to-purple-500/20 rounded-xl p-3 transform hover:scale-105 transition-transform duration-200">
                      <div class="text-center">
                        <i class="fas fa-fire text-xl text-orange-500 mb-1"></i>
                        <h5 class="text-xs font-medium text-gray-500 dark:text-gray-400">{{ t('currentStreak') }}</h5>
                        <p class="text-xl font-bold text-gray-900 dark:text-white">{{ userStats.currentStreak || 0 }}</p>
                      </div>
                    </div>
                    <div class="bg-gradient-to-br from-indigo-500/10 to-purple-500/10 dark:from-indigo-500/20 dark:to-purple-500/20 rounded-xl p-3 transform hover:scale-105 transition-transform duration-200">
                      <div class="text-center">
                        <i class="fas fa-star text-xl text-purple-500 mb-1"></i>
                        <h5 class="text-xs font-medium text-gray-500 dark:text-gray-400">{{ t('bestStreak') }}</h5>
                        <p class="text-xl font-bold text-gray-900 dark:text-white">{{ userStats.bestStreak || 0 }}</p>
                      </div>
                    </div>
                    <div class="bg-gradient-to-br from-indigo-500/10 to-purple-500/10 dark:from-indigo-500/20 dark:to-purple-500/20 rounded-xl p-3 transform hover:scale-105 transition-transform duration-200">
                      <div class="text-center">
                        <i class="fas fa-chart-line text-xl text-green-500 mb-1"></i>
                        <h5 class="text-xs font-medium text-gray-500 dark:text-gray-400">{{ t('winRate') }}</h5>
                        <p class="text-xl font-bold text-gray-900 dark:text-white">{{ formatPercentage(userStats.winRate) }}</p>
                      </div>
                    </div>
                  </div>

                  <!-- Recent Games -->
                  <div class="bg-white/50 dark:bg-gray-800/50 rounded-xl p-4 backdrop-blur-sm">
                    <h4 class="text-base font-bold bg-gradient-to-r from-indigo-500 to-purple-500 bg-clip-text text-transparent mb-3">
                      {{ t('recentGames') }}
                    </h4>
                    <div class="overflow-x-auto max-h-[200px] overflow-y-auto">
                      <table class="w-full">
                        <thead>
                          <tr class="border-b-2 border-gray-200 dark:border-gray-700">
                            <th class="pb-3 text-sm font-medium text-gray-500 dark:text-gray-400">{{ t('date') }}</th>
                            <th class="pb-3 text-sm font-medium text-gray-500 dark:text-gray-400">{{ t('word') }}</th>
                            <th class="pb-3 text-sm font-medium text-gray-500 dark:text-gray-400">{{ t('attempts') }}</th>
                            <th class="pb-3 text-sm font-medium text-gray-500 dark:text-gray-400">{{ t('result') }}</th>
                          </tr>
                        </thead>
                        <tbody>
                          <tr v-if="!recentGames.length" class="text-center">
                            <td colspan="4" class="py-8">
                              <i class="fas fa-gamepad text-4xl text-gray-300 dark:text-gray-600 mb-2"></i>
                              <p class="text-gray-500 dark:text-gray-400">{{ t('noGamesYet') }}</p>
                            </td>
                          </tr>
                          <tr v-for="game in recentGames" :key="game.id" 
                              class="border-b border-gray-100 dark:border-gray-700/50 hover:bg-gray-50 dark:hover:bg-gray-700/30 transition-colors duration-150">
                            <td class="py-3 text-gray-900 dark:text-white">{{ formatDate(game.date) }}</td>
                            <td class="py-3">
                              <div class="flex justify-center space-x-1">
                                <span v-for="(letter, index) in game.word" :key="index"
                                      class="w-7 h-7 flex items-center justify-center text-sm font-medium bg-gray-100 dark:bg-gray-700 rounded">
                                  {{ letter }}
                                </span>
                              </div>
                            </td>
                            <td class="py-3 text-center">
                              <div class="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium"
                                   :class="getAttemptsClass(game.attempts)">
                                {{ game.attempts }}/6
                              </div>
                            </td>
                            <td class="py-3">
                              <span :class="[
                                'inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium',
                                game.won ? 'bg-green-100 text-green-800 dark:bg-green-800/30 dark:text-green-400' : 'bg-red-100 text-red-800 dark:bg-red-800/30 dark:text-red-400'
                              ]">
                                <i :class="['fas', game.won ? 'fa-check' : 'fa-times', 'mr-1']"></i>
                                {{ game.won ? t('won') : t('lost') }}
                              </span>
                            </td>
                          </tr>
                        </tbody>
                      </table>
                    </div>
                  </div>

                  <!-- Statistics Chart -->
                  <div class="bg-white/50 dark:bg-gray-800/50 rounded-xl p-4 backdrop-blur-sm">
                    <h4 class="text-base font-bold bg-gradient-to-r from-indigo-500 to-purple-500 bg-clip-text text-transparent mb-3">
                      {{ t('guessDistribution') }}
                    </h4>
                    <div class="space-y-1.5">
                      <div v-for="i in 6" :key="i" class="flex items-center">
                        <span class="w-3 text-xs text-gray-500 dark:text-gray-400">{{ i }}</span>
                        <div class="flex-1 ml-2">
                          <div class="bg-gray-100 dark:bg-gray-700 rounded-full h-5 overflow-hidden">
                            <div :style="{ width: getGuessPercentage(i) + '%' }"
                                 class="h-full bg-gradient-to-r from-indigo-500 to-purple-500 transition-all duration-1000">
                            </div>
                          </div>
                        </div>
                        <span class="ml-2 text-xs text-gray-500 dark:text-gray-400">{{ getGuessCount(i) }}</span>
                      </div>
                    </div>
                  </div>
                </div>

                <!-- Footer -->
                <div class="mt-4 flex justify-end space-x-4 rtl:space-x-reverse">
                  <button
                    @click="handleLogout"
                    class="inline-flex items-center px-3 py-1.5 bg-gradient-to-r from-red-500 to-pink-500 text-white text-sm rounded-lg hover:from-red-600 hover:to-pink-600 transition-all duration-200 transform hover:scale-105"
                  >
                    <i :class="['fas fa-sign-out-alt', dir === 'rtl' ? 'ml-1.5' : 'mr-1.5']"></i>
                    {{ t('logout') }}
                  </button>
                </div>
              </div>
            </DialogPanel>
          </TransitionChild>
        </div>
      </div>
    </Dialog>
  </TransitionRoot>
</template>

<script setup>
import { ref, computed, onMounted, watch } from 'vue'
import { Dialog, DialogPanel, DialogTitle, TransitionChild, TransitionRoot } from '@headlessui/vue'
import { useAuthStore } from '~/stores/auth'
import { useProfileStore } from '~/stores/profile'
import { useTranslations } from '~/composables/useTranslations'
import StatisticsCard from '~/components/Statistics/StatisticsCard.vue'

const props = defineProps({
  isOpen: {
    type: Boolean,
    required: true
  }
})

const emit = defineEmits(['close'])

const authStore = useAuthStore()
const profileStore = useProfileStore()
const router = useRouter()
const { t, dir } = useTranslations()

const userStats = computed(() => ({
  score: profileStore.profile.stats.totalGames * 100,
  currentStreak: profileStore.profile.stats.currentStreak,
  bestStreak: profileStore.profile.stats.maxStreak,
  winRate: profileStore.winRate,
  guessDistribution: profileStore.profile.stats.guessDistribution || {
    1: 0, 2: 0, 3: 0, 4: 0, 5: 0, 6: 0
  }
}))

const recentGames = computed(() => profileStore.profile.recentGames)

onMounted(async () => {
  if (props.isOpen) {
    await Promise.all([
      profileStore.fetchProfile(),
      profileStore.fetchRecentGames()
    ])
  }
})

watch(() => props.isOpen, async (newValue) => {
  if (newValue) {
    await Promise.all([
      profileStore.fetchProfile(),
      profileStore.fetchRecentGames()
    ])
  }
})

watch(() => authStore.isAuthenticated, (isAuthenticated) => {
  if (!isAuthenticated) {
    isProfileModalOpen.value = false
    isDailyModalOpen.value = false
    isLeaderboardModalOpen.value = false
  }
})

const closeModal = () => {
  emit('close')
}

const handleLogout = async () => {
  await authStore.logout()
  closeModal()
  router.push('/login')
}

const formatDate = (date) => {
  return new Date(date).toLocaleDateString(dir.value === 'rtl' ? 'fa-IR' : 'en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric'
  })
}

const formatPercentage = (value) => {
  return `${Math.round(value)}%`
}

const getAttemptsClass = (attempts) => {
  const classes = {
    base: 'bg-opacity-50 dark:bg-opacity-30',
    1: 'bg-green-100 text-green-800 dark:bg-green-800 dark:text-green-400',
    2: 'bg-teal-100 text-teal-800 dark:bg-teal-800 dark:text-teal-400',
    3: 'bg-blue-100 text-blue-800 dark:bg-blue-800 dark:text-blue-400',
    4: 'bg-purple-100 text-purple-800 dark:bg-purple-800 dark:text-purple-400',
    5: 'bg-pink-100 text-pink-800 dark:bg-pink-800 dark:text-pink-400',
    6: 'bg-red-100 text-red-800 dark:bg-red-800 dark:text-red-400'
  }
  return `${classes[attempts]} ${classes.base}`
}

const getGuessCount = (number) => {
  return userStats.value.guessDistribution[number] || 0
}

const totalGuesses = computed(() => {
  return Object.values(userStats.value.guessDistribution).reduce((a, b) => a + b, 0)
})

const getGuessPercentage = (number) => {
  const count = getGuessCount(number)
  return totalGuesses.value ? (count / totalGuesses.value) * 100 : 0
}
</script>

<style scoped>
.animate-pulse-slow {
  animation: pulse 3s cubic-bezier(0.4, 0, 0.6, 1) infinite;
}

@keyframes pulse {
  0%, 100% {
    opacity: 1;
  }
  50% {
    opacity: .7;
  }
}
</style> 