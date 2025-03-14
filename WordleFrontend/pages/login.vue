<template>
  <div :dir="dir" class="min-h-screen flex items-center justify-center theme-bg-secondary p-4">
    <!-- Language Switcher -->
    <div class="fixed top-4 left-1/2 -translate-x-1/2 z-10">
      <button
        @click="toggleLocale"
        class="px-6 py-2 theme-bg-primary rounded-full shadow-lg hover:shadow-xl transition-all duration-300 flex items-center space-x-2 group"
        :class="{ 'flex-row-reverse space-x-reverse': dir === 'rtl' }"
      >
        <span class="theme-text-primary font-medium">
          {{ locale === 'fa' ? 'English' : 'فارسی' }}
        </span>
        <svg 
          class="w-5 h-5 theme-text-secondary transform group-hover:rotate-180 transition-transform duration-300"
          :class="{ 'rotate-180': dir === 'rtl' }"
          fill="none"
          stroke="currentColor"
          viewBox="0 0 24 24"
        >
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
        </svg>
      </button>
    </div>

    <div class="max-w-md w-full perspective-1000">
      <div class="game-card theme-bg-primary rounded-2xl p-8 shadow-2xl transform hover:scale-105 transition-all duration-300">
        <!-- Logo/Title Section -->
        <div class="text-center mb-8 animate-fade-in">
          <h1 class="text-4xl font-bold theme-text-primary">
            WORDLE
          </h1>
          <p class="text-lg theme-text-secondary mt-2">{{ t('welcome') }}</p>
        </div>

        <!-- Login Form -->
        <form @submit.prevent="handleSubmit" class="space-y-6">
          <!-- Error Message -->
          <div v-if="error" class="bg-red-100 dark:bg-red-900/50 border border-red-500/50 rounded-lg p-4 animate-shake">
            <p class="text-red-600 dark:text-red-200 text-sm text-center">{{ error }}</p>
          </div>

          <!-- Username Input -->
          <div class="game-input-container">
            <input
              id="username"
              v-model="form.username"
              type="text"
              required
              :placeholder="t('username')"
              class="theme-input w-full rounded-lg px-4 py-3 transition-all duration-300"
            />
            <div class="input-glow"></div>
          </div>

          <!-- Password Input -->
          <div class="game-input-container">
            <input
              id="password"
              v-model="form.password"
              type="password"
              required
              :placeholder="t('password')"
              class="theme-input w-full rounded-lg px-4 py-3 transition-all duration-300"
            />
            <div class="input-glow"></div>
          </div>

          <!-- Login Button -->
          <button
            type="submit"
            :disabled="loading"
            class="theme-button-primary w-full text-white font-bold py-3 px-4 rounded-lg transform hover:scale-105 transition-all duration-300 shadow-lg"
          >
            <span class="flex items-center justify-center">
              <svg
                v-if="loading"
                class="animate-spin -ml-1 mr-3 h-5 w-5 text-white"
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 24 24"
              >
                <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
              </svg>
              {{ loading ? t('loading') : t('login') }}
            </span>
          </button>

          <!-- Guest Button -->
          <button
            type="button"
            @click="playAsGuest"
            class="w-full flex justify-center py-3 px-4 rounded-lg shadow-sm text-sm font-medium theme-button-secondary transform hover:scale-105 transition-all duration-300"
          >
            {{ t('playAsGuest') }}
          </button>

          <!-- Register Link -->
          <div class="text-center">
            <NuxtLink
              to="/register"
              class="theme-text-secondary hover:theme-text-primary transition-colors duration-300 text-sm inline-flex items-center group"
            >
              {{ t('noAccount') }}
              <svg 
                class="w-4 h-4 ml-1 transform group-hover:translate-x-1 transition-transform duration-300"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
              </svg>
            </NuxtLink>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
definePageMeta({
  layout: 'auth'
})

const { t, locale, setLocale, dir } = useTranslations()
const router = useRouter()
const auth = useAuth()
const { login, loading, error } = auth

const form = reactive({
  username: '',
  password: ''
})

const toggleLocale = () => {
  setLocale(locale.value === 'fa' ? 'en' : 'fa')
}

const handleSubmit = async () => {
  const success = await login(form.username, form.password)
  if (success) {
    router.push('/')
  }
}

const playAsGuest = () => {
  const success = auth.playAsGuest()
  if (success) {
    router.push('/')
  }
}
</script>

<style scoped>
.perspective-1000 {
  perspective: 1000px;
}

.game-card {
  transform-style: preserve-3d;
  transition: transform 0.3s ease;
}

.game-card:hover {
  transform: scale(1.02);
}

.game-input-container {
  position: relative;
  overflow: hidden;
}

.input-glow {
  position: absolute;
  top: 0;
  left: -100%;
  width: 50%;
  height: 100%;
  background: linear-gradient(
    90deg,
    transparent,
    rgba(255, 255, 255, 0.1),
    transparent
  );
  animation: inputGlow 3s linear infinite;
}

.animate-fade-in {
  animation: fadeIn 0.5s ease-out;
}

.animate-shake {
  animation: shake 0.5s cubic-bezier(.36,.07,.19,.97) both;
}

@keyframes inputGlow {
  0% {
    left: -100%;
  }
  100% {
    left: 200%;
  }
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(-20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@keyframes shake {
  10%, 90% {
    transform: translateX(-1px);
  }
  20%, 80% {
    transform: translateX(2px);
  }
  30%, 50%, 70% {
    transform: translateX(-4px);
  }
  40%, 60% {
    transform: translateX(4px);
  }
}
</style> 