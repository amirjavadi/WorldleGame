<template>
  <Modal v-model="isModalOpen">
    <div class="relative">
      <button
        @click="handleClose"
        :class="[
          'absolute top-0',
          dir === 'rtl' ? 'left-0' : 'right-0',
          'text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-200'
        ]"
      >
        <i class="fas fa-times text-xl"></i>
      </button>

      <div class="text-center mb-8">
        <div class="relative inline-block">
          <i class="fas fa-calendar-day text-4xl text-transparent bg-clip-text bg-gradient-to-r from-indigo-500 via-purple-500 to-pink-500 animate-bounce-slow"></i>
          <div class="absolute -top-1 -right-1 w-3 h-3 bg-yellow-400 rounded-full animate-ping"></div>
        </div>
        <h2 class="text-2xl font-bold bg-gradient-to-r from-indigo-500 via-purple-500 to-pink-500 bg-clip-text text-transparent mt-4 mb-2">
          {{ t('dailyChallenge') }}
        </h2>
        <p class="text-gray-600 dark:text-gray-300">
          {{ t('dailyChallengeDescription') }}
        </p>
      </div>

      <template v-if="!authStore.isLoggedIn || authStore.isGuest">
        <div class="text-center p-6 bg-gradient-to-br from-yellow-50 to-orange-50 dark:from-yellow-900/20 dark:to-orange-900/20 rounded-lg border border-yellow-200 dark:border-yellow-800 shadow-lg transform hover:scale-[1.02] transition-all duration-300">
          <div class="relative inline-block mb-4">
            <i class="fas fa-lock text-4xl text-yellow-500 animate-bounce-slow"></i>
            <div class="absolute -top-1 -right-1 w-3 h-3 bg-red-400 rounded-full animate-ping"></div>
          </div>
          <h3 class="text-xl font-semibold text-yellow-800 dark:text-yellow-200 mb-2">
            {{ t('guestAccessDenied') }}
          </h3>
          <p class="text-yellow-600 dark:text-yellow-300 mb-4">
            {{ t('pleaseLoginToAccessDaily') }}
          </p>
          <div class="flex justify-center space-x-4 rtl:space-x-reverse">
            <NuxtLink
              to="/login"
              class="group px-6 py-3 bg-gradient-to-r from-yellow-500 to-orange-500 text-white rounded-lg transition-all duration-300 hover:shadow-lg hover:shadow-yellow-500/25 hover:-translate-y-0.5"
              @click="handleClose"
            >
              <i class="fas fa-sign-in-alt mr-2 group-hover:animate-bounce-horizontal"></i>
              {{ t('login') }}
            </NuxtLink>
            <NuxtLink
              to="/register"
              class="group px-6 py-3 bg-gradient-to-r from-yellow-100 to-orange-100 text-yellow-800 rounded-lg transition-all duration-300 hover:shadow-lg hover:shadow-yellow-200/25 hover:-translate-y-0.5 dark:from-yellow-900 dark:to-orange-900 dark:text-yellow-100"
              @click="handleClose"
            >
              <i class="fas fa-user-plus mr-2 group-hover:animate-bounce-horizontal"></i>
              {{ t('register') }}
            </NuxtLink>
          </div>
        </div>
      </template>

      <template v-else>
        <div v-if="dailyStore.loading" class="flex justify-center items-center py-12">
          <div class="relative">
            <div class="w-16 h-16 border-4 border-indigo-200 border-t-indigo-500 rounded-full animate-spin"></div>
            <div class="absolute inset-0 flex items-center justify-center">
              <i class="fas fa-gamepad text-indigo-500 animate-pulse"></i>
            </div>
          </div>
        </div>

        <template v-else>
          <!-- Timer Section -->
          <div v-if="!dailyStore.canPlay" class="mb-8">
            <div class="text-center">
              <h3 class="text-xl font-semibold text-transparent bg-clip-text bg-gradient-to-r from-indigo-500 via-purple-500 to-pink-500 mb-4">
                {{ t('timeUntilNextChallenge') }}
              </h3>
              <div class="flex justify-center items-center space-x-4 rtl:space-x-reverse">
                <div class="group flex flex-col items-center p-4 bg-gradient-to-br from-indigo-50 to-purple-50 dark:from-indigo-900/30 dark:to-purple-900/30 rounded-lg transform transition-all duration-300 hover:scale-110 hover:shadow-lg hover:shadow-indigo-500/25">
                  <span class="text-3xl font-bold text-indigo-600 dark:text-indigo-400 group-hover:animate-bounce-slow">{{ hours }}</span>
                  <span class="text-sm text-indigo-600 dark:text-indigo-400">{{ t('hours') }}</span>
                </div>
                <span class="text-2xl font-bold text-indigo-400 dark:text-indigo-500 animate-pulse">:</span>
                <div class="group flex flex-col items-center p-4 bg-gradient-to-br from-purple-50 to-pink-50 dark:from-purple-900/30 dark:to-pink-900/30 rounded-lg transform transition-all duration-300 hover:scale-110 hover:shadow-lg hover:shadow-purple-500/25">
                  <span class="text-3xl font-bold text-purple-600 dark:text-purple-400 group-hover:animate-bounce-slow">{{ minutes }}</span>
                  <span class="text-sm text-purple-600 dark:text-purple-400">{{ t('minutes') }}</span>
                </div>
                <span class="text-2xl font-bold text-purple-400 dark:text-purple-500 animate-pulse">:</span>
                <div class="group flex flex-col items-center p-4 bg-gradient-to-br from-pink-50 to-rose-50 dark:from-pink-900/30 dark:to-rose-900/30 rounded-lg transform transition-all duration-300 hover:scale-110 hover:shadow-lg hover:shadow-pink-500/25">
                  <span class="text-3xl font-bold text-pink-600 dark:text-pink-400 group-hover:animate-bounce-slow">{{ seconds }}</span>
                  <span class="text-sm text-pink-600 dark:text-pink-400">{{ t('seconds') }}</span>
                </div>
              </div>
            </div>
          </div>

          <!-- Game Section -->
          <div v-else class="space-y-6">
            <div class="bg-gradient-to-br from-indigo-50 via-purple-50 to-pink-50 dark:from-indigo-900/30 dark:via-purple-900/30 dark:to-pink-900/30 rounded-lg p-6 transform transition-all duration-300 hover:shadow-lg hover:shadow-indigo-500/25">
              <div class="text-center">
                <h3 class="text-xl font-semibold text-transparent bg-clip-text bg-gradient-to-r from-indigo-500 via-purple-500 to-pink-500">
                  {{ t('guessWord') }}
                </h3>
                <Challenge 
                  v-if="dailyStore.word" 
                  :word="dailyStore.word"
                  :is-daily="true"
                  @guess-submitted="handleGuessSubmitted"
                />
              </div>
            </div>
          </div>
        </template>

        <!-- Error Message -->
        <div v-if="dailyStore.error" class="mt-4 p-4 bg-gradient-to-br from-red-50 to-pink-50 dark:from-red-900/30 dark:to-pink-900/30 rounded-lg border border-red-200 dark:border-red-800">
          <p class="text-red-600 dark:text-red-400 text-center">
            <i class="fas fa-exclamation-circle mr-2 animate-bounce"></i>
            {{ dailyStore.error }}
          </p>
        </div>
      </template>
    </div>
  </Modal>
</template>

<script setup>
import { ref, onMounted, onUnmounted, watch } from 'vue'
import { useDailyStore } from '~/stores/daily'
import { useTranslations } from '~/composables/useTranslations'
import { useAuthStore } from '~/stores/auth'
import Challenge from '~/components/DailyChallenge/Challenge.vue'
import Modal from '~/components/UI/Modal.vue'

const props = defineProps({
  modelValue: {
    type: Boolean,
    default: false
  }
})

const emit = defineEmits(['update:modelValue'])

const dailyStore = useDailyStore()
const authStore = useAuthStore()
const { t, dir } = useTranslations()

const isModalOpen = ref(props.modelValue)

watch(() => props.modelValue, (newVal) => {
  isModalOpen.value = newVal
})

watch(isModalOpen, (newVal) => {
  emit('update:modelValue', newVal)
  if (!newVal) {
    dailyStore.resetError()
  }
})

const hours = ref('00')
const minutes = ref('00')
const seconds = ref('00')
let timerInterval

const updateTimer = () => {
  const timeLeft = dailyStore.timeUntilNextChallenge
  if (!timeLeft) return

  const h = Math.floor(timeLeft / 3600000)
  const m = Math.floor((timeLeft % 3600000) / 60000)
  const s = Math.floor((timeLeft % 60000) / 1000)

  hours.value = h.toString().padStart(2, '0')
  minutes.value = m.toString().padStart(2, '0')
  seconds.value = s.toString().padStart(2, '0')
}

const handleClose = () => {
  clearInterval(timerInterval)
  dailyStore.resetError()
  emit('update:modelValue', false)
}

const handleGuessSubmitted = async (guess) => {
  const result = await dailyStore.submitGuess(guess)
  if (result?.isGameOver) {
    if (dailyStore.timeUntilNextChallenge) {
      startCountdown(dailyStore.timeUntilNextChallenge)
    }
  }
  return result
}

const initializeDaily = async () => {
  if (!authStore.isLoggedIn || authStore.isGuest) {
    dailyStore.resetState();
    handleClose();
    return;
  }

  if (!authStore.token) {
    dailyStore.resetState();
    handleClose();
    return;
  }

  try {
    // Validate token before initializing
    const isValid = await authStore.checkAuth();
    if (!isValid) {
      handleClose();
      return;
    }

    if (!timerInterval) {
      timerInterval = setInterval(updateTimer, 1000);
      updateTimer();
    }
    await dailyStore.fetchDailyWord();
  } catch (error) {
    console.error('Error initializing daily challenge:', error);
    handleClose();
  }
}

watch(() => props.modelValue, async (isOpen) => {
  if (isOpen) {
    if (!authStore.isLoggedIn || authStore.isGuest || !authStore.token) {
      handleClose();
      return;
    }
    await initializeDaily();
  } else {
    if (timerInterval) {
      clearInterval(timerInterval);
      timerInterval = null;
    }
    dailyStore.resetState();
  }
}, { immediate: true })

watch([() => authStore.isLoggedIn, () => authStore.token, () => authStore.isGuest], async ([isLoggedIn, token, isGuest]) => {
  if (props.modelValue) {
    if (!isLoggedIn || isGuest || !token) {
      handleClose();
      return;
    }
    await initializeDaily();
  }
})

onMounted(() => {
  if (props.modelValue) {
    initializeDaily();
  }
})

onUnmounted(() => {
  if (timerInterval) {
    clearInterval(timerInterval);
    timerInterval = null;
  }
  dailyStore.resetState();
})
</script>

<style scoped>
@keyframes bounce-slow {
  0%, 100% {
    transform: translateY(0);
  }
  50% {
    transform: translateY(-10px);
  }
}

.animate-bounce-slow {
  animation: bounce-slow 2s infinite;
}

@keyframes bounce-horizontal {
  0%, 100% {
    transform: translateX(0);
  }
  50% {
    transform: translateX(3px);
  }
}

.group-hover\:animate-bounce-horizontal:hover {
  animation: bounce-horizontal 1s infinite;
}
</style> 