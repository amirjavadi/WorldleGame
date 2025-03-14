<template>
  <div :dir="dir" class="min-h-screen bg-gradient-to-br from-gray-100 to-gray-200 dark:from-gray-900 dark:to-gray-800 p-4">
    <div class="max-w-4xl mx-auto">
      <!-- Profile Card -->
      <div class="bg-white dark:bg-gray-800 rounded-2xl p-8 shadow-2xl mb-8">
        <div class="flex items-center justify-between mb-8">
          <h1 class="text-3xl font-bold text-gray-900 dark:text-white">{{ t('profile') }}</h1>
          <button
            @click="logout"
            class="px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 transition-colors duration-300"
          >
            {{ t('logout') }}
          </button>
        </div>

        <!-- User Info -->
        <div class="grid grid-cols-1 md:grid-cols-2 gap-8">
          <div>
            <h2 class="text-xl font-semibold text-gray-800 dark:text-gray-200 mb-4">{{ t('userInfo') }}</h2>
            <div class="space-y-4">
              <div>
                <label class="block text-sm font-medium text-gray-600 dark:text-gray-400">{{ t('username') }}</label>
                <p class="mt-1 text-lg text-gray-900 dark:text-white">{{ user?.username || t('guest') }}</p>
              </div>
              <div v-if="user?.email">
                <label class="block text-sm font-medium text-gray-600 dark:text-gray-400">{{ t('email') }}</label>
                <p class="mt-1 text-lg text-gray-900 dark:text-white">{{ user.email }}</p>
              </div>
            </div>
          </div>

          <!-- Game Stats -->
          <div>
            <h2 class="text-xl font-semibold text-gray-800 dark:text-gray-200 mb-4">{{ t('statistics') }}</h2>
            <div class="grid grid-cols-2 gap-4">
              <div class="bg-gray-50 dark:bg-gray-700 p-4 rounded-lg">
                <p class="text-sm text-gray-600 dark:text-gray-400">{{ t('gamesPlayed') }}</p>
                <p class="text-2xl font-bold text-gray-900 dark:text-white">0</p>
              </div>
              <div class="bg-gray-50 dark:bg-gray-700 p-4 rounded-lg">
                <p class="text-sm text-gray-600 dark:text-gray-400">{{ t('winRate') }}</p>
                <p class="text-2xl font-bold text-gray-900 dark:text-white">0%</p>
              </div>
              <div class="bg-gray-50 dark:bg-gray-700 p-4 rounded-lg">
                <p class="text-sm text-gray-600 dark:text-gray-400">{{ t('currentStreak') }}</p>
                <p class="text-2xl font-bold text-gray-900 dark:text-white">0</p>
              </div>
              <div class="bg-gray-50 dark:bg-gray-700 p-4 rounded-lg">
                <p class="text-sm text-gray-600 dark:text-gray-400">{{ t('bestStreak') }}</p>
                <p class="text-2xl font-bold text-gray-900 dark:text-white">0</p>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Recent Games -->
      <div class="bg-white dark:bg-gray-800 rounded-2xl p-8 shadow-2xl">
        <h2 class="text-xl font-semibold text-gray-800 dark:text-gray-200 mb-4">{{ t('recentGames') }}</h2>
        <div class="overflow-x-auto">
          <table class="w-full">
            <thead>
              <tr class="text-left border-b border-gray-200 dark:border-gray-700">
                <th class="pb-3 text-gray-600 dark:text-gray-400">{{ t('date') }}</th>
                <th class="pb-3 text-gray-600 dark:text-gray-400">{{ t('word') }}</th>
                <th class="pb-3 text-gray-600 dark:text-gray-400">{{ t('attempts') }}</th>
                <th class="pb-3 text-gray-600 dark:text-gray-400">{{ t('result') }}</th>
              </tr>
            </thead>
            <tbody>
              <tr class="text-gray-500 dark:text-gray-400">
                <td class="py-3">-</td>
                <td class="py-3">-</td>
                <td class="py-3">-</td>
                <td class="py-3">-</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
definePageMeta({
  middleware: ['auth']
})

const { t, dir } = useTranslations()
const { user, logout } = useAuth()
const router = useRouter()

const handleLogout = () => {
  logout()
  router.push('/login')
}
</script> 