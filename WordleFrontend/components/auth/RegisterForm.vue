<template>
  <div class="w-full max-w-md mx-auto p-6 bg-white dark:bg-gray-800 rounded-lg shadow-md">
    <h2 class="text-2xl font-bold text-center mb-6">{{ $t('register') }}</h2>
    
    <form @submit.prevent="handleSubmit" class="space-y-4">
      <div>
        <label for="email" class="block text-sm font-medium mb-1">{{ $t('email') }}</label>
        <input
          id="email"
          v-model="email"
          type="email"
          class="input"
          :class="{ 'border-red-500': errors.email }"
          required
        />
        <p v-if="errors.email" class="mt-1 text-sm text-red-500">{{ errors.email }}</p>
      </div>

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

      <div>
        <label for="confirmPassword" class="block text-sm font-medium mb-1">{{ $t('confirmPassword') }}</label>
        <input
          id="confirmPassword"
          v-model="confirmPassword"
          type="password"
          class="input"
          :class="{ 'border-red-500': errors.confirmPassword }"
          required
        />
        <p v-if="errors.confirmPassword" class="mt-1 text-sm text-red-500">{{ errors.confirmPassword }}</p>
      </div>

      <button
        type="submit"
        class="btn btn-primary w-full"
        :disabled="loading"
      >
        <span v-if="loading">{{ $t('loading') }}</span>
        <span v-else>{{ $t('register') }}</span>
      </button>

      <p class="text-center text-sm">
        {{ $t('haveAccount') }}
        <NuxtLink to="/login" class="text-blue-600 hover:text-blue-700">
          {{ $t('login') }}
        </NuxtLink>
      </p>
    </form>
  </div>
</template>

<script setup>
const { t } = useI18n()
const authStore = useAuthStore()
const router = useRouter()

const email = ref('')
const username = ref('')
const password = ref('')
const confirmPassword = ref('')
const loading = ref(false)
const errors = ref({})

async function handleSubmit() {
  loading.value = true
  errors.value = {}

  if (password.value !== confirmPassword.value) {
    errors.value.confirmPassword = t('passwordsDoNotMatch')
    loading.value = false
    return
  }

  try {
    await authStore.register(email.value, username.value, password.value)
    router.push('/')
  } catch (error) {
    if (error.response?.data?.errors) {
      errors.value = error.response.data.errors
    } else {
      errors.value = { general: t('registerError') }
    }
  } finally {
    loading.value = false
  }
}
</script> 