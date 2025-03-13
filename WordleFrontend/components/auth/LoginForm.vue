<template>
  <div class="w-full max-w-md mx-auto p-6 bg-white dark:bg-gray-800 rounded-lg shadow-md">
    <h2 class="text-2xl font-bold text-center mb-6">{{ $t('login') }}</h2>
    
    <form @submit.prevent="handleSubmit" class="space-y-4">
      <div>
        <label for="username" class="block text-sm font-medium mb-1">{{ $t('username') }}</label>
        <input
          id="username"
          v-model="username"
          type="text"
          class="input"
          :class="{ 'border-red-500': errors.username }"
          required
        />
        <p v-if="errors.username" class="mt-1 text-sm text-red-500">{{ errors.username }}</p>
      </div>

      <div>
        <label for="password" class="block text-sm font-medium mb-1">{{ $t('password') }}</label>
        <input
          id="password"
          v-model="password"
          type="password"
          class="input"
          :class="{ 'border-red-500': errors.password }"
          required
        />
        <p v-if="errors.password" class="mt-1 text-sm text-red-500">{{ errors.password }}</p>
      </div>

      <button
        type="submit"
        class="btn btn-primary w-full"
        :disabled="loading"
      >
        <span v-if="loading">{{ $t('loading') }}</span>
        <span v-else>{{ $t('login') }}</span>
      </button>

      <p class="text-center text-sm">
        {{ $t('noAccount') }}
        <NuxtLink to="/register" class="text-blue-600 hover:text-blue-700">
          {{ $t('register') }}
        </NuxtLink>
      </p>
    </form>
  </div>
</template>

<script setup>
const { t } = useI18n()
const authStore = useAuthStore()
const router = useRouter()

const username = ref('')
const password = ref('')
const loading = ref(false)
const errors = ref({})

async function handleSubmit() {
  loading.value = true
  errors.value = {}

  try {
    await authStore.login(username.value, password.value)
    router.push('/')
  } catch (error) {
    if (error.response?.data?.errors) {
      errors.value = error.response.data.errors
    } else {
      errors.value = { general: t('loginError') }
    }
  } finally {
    loading.value = false
  }
}
</script> 