<template>
  <TransitionRoot appear :show="true" as="template">
    <Dialog as="div" @close="$emit('close')" class="relative z-50">
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
            <DialogPanel :dir="dir" class="w-full max-w-xl transform overflow-hidden rounded-xl bg-gradient-to-br from-white via-gray-50 to-gray-100 dark:from-gray-800 dark:via-gray-900 dark:to-gray-950 p-4 shadow-[0_0_30px_rgba(79,70,229,0.1)] dark:shadow-[0_0_30px_rgba(79,70,229,0.2)] transition-all">
              <div class="relative">
                <!-- Close Button -->
                <button
                  @click="$emit('close')"
                  :class="[
                    'absolute top-2 p-2 text-gray-400 hover:text-gray-500 dark:text-gray-500 dark:hover:text-gray-400 transition-colors duration-200 hover:rotate-90 transform',
                    dir === 'rtl' ? 'left-2' : 'right-2'
                  ]"
                >
                  <span class="sr-only">{{ t('close') }}</span>
                  <i class="fas fa-times text-xl"></i>
                </button>

                <!-- Header -->
                <div class="text-center mb-4">
                  <div class="inline-block p-1.5 rounded-full bg-gradient-to-r from-yellow-400 via-amber-400 to-yellow-500 mb-2 animate-pulse-slow">
                    <div class="bg-white dark:bg-gray-800 rounded-full p-2 animate-float">
                      <i class="fas fa-trophy text-2xl bg-gradient-to-r from-yellow-400 via-amber-400 to-yellow-500 bg-clip-text text-transparent animate-shine"></i>
                    </div>
                  </div>
                  <DialogTitle as="h3" class="text-2xl font-bold bg-gradient-to-r from-yellow-400 via-amber-400 to-yellow-500 bg-clip-text text-transparent mb-1">
                    {{ t('dailyChallenge') }}
                  </DialogTitle>
                  <p class="text-xs text-gray-600 dark:text-gray-300">{{ t('dailyChallengeDescription') }}</p>
                </div>

                <!-- Content -->
                <div class="space-y-4">
                  <!-- Timer -->
                  <div class="bg-gradient-to-r from-indigo-50 via-purple-50 to-pink-50 dark:from-indigo-900/20 dark:via-purple-900/20 dark:to-pink-900/20 p-4 rounded-lg shadow-md transform hover:scale-[1.01] transition-all duration-300">
                    <div class="text-center">
                      <p class="text-sm text-indigo-600 dark:text-indigo-400 mb-3">{{ t('nextChallengeIn') }}</p>
                      <div class="flex justify-center space-x-6 rtl:space-x-reverse">
                        <div class="text-center transform hover:scale-105 transition-transform duration-200">
                          <div class="relative w-16 h-16 mx-auto">
                            <svg class="w-full h-full transform -rotate-90">
                              <circle
                                class="text-gray-200 dark:text-gray-700"
                                stroke-width="3"
                                stroke="currentColor"
                                fill="transparent"
                                r="30"
                                cx="32"
                                cy="32"
                              />
                              <circle
                                class="text-indigo-500 dark:text-indigo-400"
                                stroke-width="3"
                                stroke-dasharray="188"
                                stroke-dashoffset="188"
                                stroke-linecap="round"
                                stroke="currentColor"
                                fill="transparent"
                                r="30"
                                cx="32"
                                cy="32"
                                :style="{ strokeDashoffset: 188 - (188 * hours) / 24 }"
                              />
                            </svg>
                            <div class="absolute inset-0 flex items-center justify-center">
                              <div class="text-2xl font-bold text-indigo-600 dark:text-indigo-400">{{ hours }}</div>
                            </div>
                          </div>
                          <div class="text-xs text-indigo-500 dark:text-indigo-500 mt-1">{{ t('hours') }}</div>
                        </div>
                        <div class="text-center transform hover:scale-105 transition-transform duration-200">
                          <div class="relative w-16 h-16 mx-auto">
                            <svg class="w-full h-full transform -rotate-90">
                              <circle
                                class="text-gray-200 dark:text-gray-700"
                                stroke-width="3"
                                stroke="currentColor"
                                fill="transparent"
                                r="30"
                                cx="32"
                                cy="32"
                              />
                              <circle
                                class="text-indigo-500 dark:text-indigo-400"
                                stroke-width="3"
                                stroke-dasharray="188"
                                stroke-dashoffset="188"
                                stroke-linecap="round"
                                stroke="currentColor"
                                fill="transparent"
                                r="30"
                                cx="32"
                                cy="32"
                                :style="{ strokeDashoffset: 188 - (188 * minutes) / 60 }"
                              />
                            </svg>
                            <div class="absolute inset-0 flex items-center justify-center">
                              <div class="text-2xl font-bold text-indigo-600 dark:text-indigo-400">{{ minutes }}</div>
                            </div>
                          </div>
                          <div class="text-xs text-indigo-500 dark:text-indigo-500 mt-1">{{ t('minutes') }}</div>
                        </div>
                        <div class="text-center transform hover:scale-105 transition-transform duration-200">
                          <div class="relative w-16 h-16 mx-auto">
                            <svg class="w-full h-full transform -rotate-90">
                              <circle
                                class="text-gray-200 dark:text-gray-700"
                                stroke-width="3"
                                stroke="currentColor"
                                fill="transparent"
                                r="30"
                                cx="32"
                                cy="32"
                              />
                              <circle
                                class="text-indigo-500 dark:text-indigo-400"
                                stroke-width="3"
                                stroke-dasharray="188"
                                stroke-dashoffset="188"
                                stroke-linecap="round"
                                stroke="currentColor"
                                fill="transparent"
                                r="30"
                                cx="32"
                                cy="32"
                                :style="{ strokeDashoffset: 188 - (188 * seconds) / 60 }"
                              />
                            </svg>
                            <div class="absolute inset-0 flex items-center justify-center">
                              <div class="text-2xl font-bold text-indigo-600 dark:text-indigo-400">{{ seconds }}</div>
                            </div>
                          </div>
                          <div class="text-xs text-indigo-500 dark:text-indigo-500 mt-1">{{ t('seconds') }}</div>
                        </div>
                      </div>
                    </div>
                  </div>

                  <!-- Challenge Info -->
                  <div v-if="!isInProgress" class="bg-white/50 dark:bg-gray-800/50 rounded-xl p-4 backdrop-blur-sm">
                    <div class="flex flex-col md:flex-row gap-4">
                      <!-- Rules Section -->
                      <div class="flex-1">
                        <h4 class="text-lg font-bold text-gray-900 dark:text-white mb-3">{{ t('dailyChallengeRules') }}</h4>
                        <ul class="text-sm text-gray-600 dark:text-gray-300 space-y-2">
                          <li class="flex items-center space-x-2 rtl:space-x-reverse">
                            <i class="fas fa-check-circle text-green-500"></i>
                            <span>{{ t('dailyChallengeRule1') }}</span>
                          </li>
                          <li class="flex items-center space-x-2 rtl:space-x-reverse">
                            <i class="fas fa-check-circle text-green-500"></i>
                            <span>{{ t('dailyChallengeRule2') }}</span>
                          </li>
                          <li class="flex items-center space-x-2 rtl:space-x-reverse">
                            <i class="fas fa-check-circle text-green-500"></i>
                            <span>{{ t('dailyChallengeRule3') }}</span>
                          </li>
                        </ul>
                      </div>

                      <!-- Winners Podium -->
                      <div class="flex-1">
                        <!-- Podium Section -->
                        <div class="flex items-end justify-center gap-2 mb-4">
                          <!-- Second Place -->
                          <div class="flex flex-col items-center">
                            <div class="w-12 h-12 rounded-full bg-gray-200 dark:bg-gray-700 flex items-center justify-center mb-1">
                              <span class="text-lg font-bold text-gray-600 dark:text-gray-300">2</span>
                            </div>
                            <div class="text-xs font-medium text-gray-600 dark:text-gray-300 truncate max-w-[80px]">
                              {{ leaderboard[1]?.username || '' }}
                            </div>
                            <div class="text-[10px] text-gray-500 dark:text-gray-400">
                              {{ leaderboard[1]?.score || 0 }}
                            </div>
                          </div>

                          <!-- First Place -->
                          <div class="flex flex-col items-center">
                            <div class="w-14 h-14 rounded-full bg-yellow-400 dark:bg-yellow-500 flex items-center justify-center mb-1">
                              <span class="text-xl font-bold text-white">1</span>
                            </div>
                            <div class="text-xs font-medium text-gray-600 dark:text-gray-300 truncate max-w-[80px]">
                              {{ leaderboard[0]?.username || '' }}
                            </div>
                            <div class="text-[10px] text-gray-500 dark:text-gray-400">
                              {{ leaderboard[0]?.score || 0 }}
                            </div>
                          </div>

                          <!-- Third Place -->
                          <div class="flex flex-col items-center">
                            <div class="w-12 h-12 rounded-full bg-amber-600 dark:bg-amber-700 flex items-center justify-center mb-1">
                              <span class="text-lg font-bold text-white">3</span>
                            </div>
                            <div class="text-xs font-medium text-gray-600 dark:text-gray-300 truncate max-w-[80px]">
                              {{ leaderboard[2]?.username || '' }}
                            </div>
                            <div class="text-[10px] text-gray-500 dark:text-gray-400">
                              {{ leaderboard[2]?.score || 0 }}
                            </div>
                          </div>
                        </div>

                        <!-- User Rank -->
                        <div v-if="userRank" class="text-center mb-4 bg-gradient-to-r from-indigo-50 to-purple-50 dark:from-indigo-900/20 dark:to-purple-900/20 rounded-lg p-3">
                          <div class="flex items-center justify-center gap-2 mb-1">
                            <i class="fas fa-user-circle text-indigo-500 dark:text-indigo-400"></i>
                            <span class="text-sm font-bold text-indigo-600 dark:text-indigo-400">{{ t('yourRank') }}</span>
                          </div>
                          <div class="text-2xl font-bold text-indigo-600 dark:text-indigo-400 mb-1">{{ userRank }}</div>
                          <div class="text-xs text-indigo-500 dark:text-indigo-400">{{ t('yourScore') }}: {{ userScore }}</div>
                        </div>

                        <!-- View All Rankings Button -->
                        <button
                          @click="showLeaderboard"
                          class="w-full py-2 px-4 bg-blue-500 hover:bg-blue-600 text-white rounded-lg transition-colors flex items-center justify-center gap-2"
                        >
                          <i class="fas fa-trophy"></i>
                          {{ t('viewAllRankings') }}
                        </button>
                      </div>
                    </div>
                  </div>

                  <!-- Start Challenge Button -->
                  <div v-if="!isInProgress" class="text-center">
                    <button
                      @click="startChallenge"
                      class="group relative inline-flex items-center px-6 py-3 bg-gradient-to-r from-yellow-400 via-amber-400 to-yellow-500 text-white text-lg rounded-lg hover:from-yellow-500 hover:via-amber-500 hover:to-yellow-600 transition-all duration-300 transform hover:scale-102 shadow-md hover:shadow-lg overflow-hidden"
                    >
                      <span class="absolute inset-0 w-full h-full bg-gradient-to-r from-yellow-400 via-amber-400 to-yellow-500 opacity-0 group-hover:opacity-100 transition-opacity duration-300"></span>
                      <span class="absolute inset-0 w-full h-full bg-[radial-gradient(circle_at_center,_var(--tw-gradient-stops))] from-white/30 via-transparent to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-300"></span>
                      <i :class="['fas fa-play text-xl relative z-10 transform group-hover:scale-105 transition-transform duration-300', dir === 'rtl' ? 'ml-2' : 'mr-2']"></i>
                      <span class="relative z-10 font-bold">{{ t('startChallenge') }}</span>
                    </button>
                  </div>

                  <!-- Game Board -->
                  <div v-if="isInProgress" class="bg-white/50 dark:bg-gray-800/50 rounded-xl p-4 backdrop-blur-sm">
                    <GameBoard
                      :word="challenge?.word?.text"
                      :guesses="guesses"
                      :max-attempts="6"
                      :show-score="true"
                      :score="currentScore"
                      :personal-best="personalBest"
                      :is-interactive="true"
                      @make-guess="handleGuess"
                    />
                  </div>
                </div>

                <!-- Footer -->
                <div class="mt-4 flex justify-end space-x-3 rtl:space-x-reverse">
                  <button
                    @click="$emit('close')"
                    class="inline-flex items-center px-4 py-2 bg-gradient-to-r from-gray-500 to-gray-600 text-white text-sm rounded-md hover:from-gray-600 hover:to-gray-700 transition-all duration-200 transform hover:scale-102"
                  >
                    <i :class="['fas fa-times', dir === 'rtl' ? 'ml-1.5' : 'mr-1.5']"></i>
                    {{ t('close') }}
                  </button>
                </div>
              </div>
            </DialogPanel>
          </TransitionChild>
        </div>
      </div>

      <!-- Add LeaderboardModal -->
      <LeaderboardModal
        v-if="showLeaderboardModal"
        :is-open="showLeaderboardModal"
        :leaderboard="leaderboard"
        @close="closeLeaderboardModal"
      />
    </Dialog>
  </TransitionRoot>
</template>

<script setup>
import { ref, computed, onMounted, onUnmounted, watch } from 'vue'
import { Dialog, DialogPanel, DialogTitle, TransitionChild, TransitionRoot } from '@headlessui/vue'
import { useTranslations } from '~/composables/useTranslations'
import { useDailyChallengeStore } from '~/stores/dailyChallenge'
import { useAuthStore } from '~/stores/auth'
import GameBoard from '~/components/GameBoard.vue'
import LeaderboardModal from './LeaderboardModal.vue'

const emit = defineEmits(['close', 'start', 'show-leaderboard'])

const { t, dir } = useTranslations()
const dailyStore = useDailyChallengeStore()
const authStore = useAuthStore()

const isAuthenticated = computed(() => authStore.isLoggedIn)
const loading = computed(() => dailyStore.loading)
const error = computed(() => dailyStore.error)
const challenge = computed(() => dailyStore.challenge)
const participation = computed(() => dailyStore.participation)
const leaderboard = computed(() => dailyStore.leaderboard)
const guesses = computed(() => dailyStore.guesses)
const currentScore = computed(() => dailyStore.currentScore)
const isInProgress = computed(() => dailyStore.isInProgress)

// محاسبه زمان باقی‌مانده
const timeUntilNext = computed(() => dailyStore.timeUntilNextChallenge)
const hours = computed(() => {
  if (!timeUntilNext.value) return '00'
  return String(Math.floor(timeUntilNext.value / (1000 * 60 * 60))).padStart(2, '0')
})
const minutes = computed(() => {
  if (!timeUntilNext.value) return '00'
  return String(Math.floor((timeUntilNext.value % (1000 * 60 * 60)) / (1000 * 60))).padStart(2, '0')
})
const seconds = computed(() => {
  if (!timeUntilNext.value) return '00'
  return String(Math.floor((timeUntilNext.value % (1000 * 60)) / 1000)).padStart(2, '0')
})

// دریافت رکورد شخصی از localStorage
const personalBest = computed(() => {
  if (process.client) {
    const best = localStorage.getItem('dailyChallengeBest')
    return best ? parseInt(best) : null
  }
  return null
})

// ذخیره رکورد شخصی در localStorage
const updatePersonalBest = (score) => {
  if (process.client) {
    const currentBest = localStorage.getItem('dailyChallengeBest')
    if (!currentBest || score > parseInt(currentBest)) {
      localStorage.setItem('dailyChallengeBest', score.toString())
    }
  }
}

// شروع تایمر در هنگام mount
onMounted(async () => {
  if (isAuthenticated.value) {
    await Promise.all([
      dailyStore.fetchTodaysChallenge(),
      dailyStore.fetchUserParticipation(),
      dailyStore.fetchLeaderboard()
    ])
  }
})

// واکنش به تغییرات timeUntilNextChallenge
watch(() => timeUntilNext.value, (newValue) => {
  if (newValue <= 0) {
    dailyStore.fetchTodaysChallenge()
  }
})

onUnmounted(() => {
  // No need to clear interval here as it's handled by the store
})

// Handle start challenge
const startChallenge = async () => {
  try {
    await dailyStore.startParticipation()
    emit('start')
  } catch (error) {
    console.error('Failed to start challenge:', error)
  }
}

// مدیریت حدس
const handleGuess = async (guess) => {
  try {
    await dailyStore.submitGuess(guess)
    if (dailyStore.hasWon) {
      updatePersonalBest(dailyStore.currentScore)
    }
  } catch (err) {
    console.error('Error submitting guess:', err)
  }
}

const showLeaderboardModal = ref(false)

const showLeaderboard = () => {
  showLeaderboardModal.value = true
  emit('show-leaderboard')
}

const closeLeaderboardModal = () => {
  showLeaderboardModal.value = false
}
</script>

<style scoped>
.animate-pulse-slow {
  animation: pulse 3s cubic-bezier(0.4, 0, 0.6, 1) infinite;
}

@keyframes pulse {
  0%, 100% {
    opacity: 1;
    transform: scale(1);
  }
  50% {
    opacity: .8;
    transform: scale(1.05);
  }
}

@keyframes float {
  0%, 100% {
    transform: translateY(0);
  }
  50% {
    transform: translateY(-15px);
  }
}

@keyframes shine {
  0% {
    background-position: -100% 0;
  }
  100% {
    background-position: 200% 0;
  }
}

.animate-float {
  animation: float 3s ease-in-out infinite;
}

.animate-shine {
  background: linear-gradient(
    90deg,
    transparent 0%,
    rgba(255, 255, 255, 0.3) 50%,
    transparent 100%
  );
  background-size: 200% 100%;
  animation: shine 2s linear infinite;
}
</style> 