<template>
  <button
    @click="handleLogout"
    class="text-gray-600 hover:text-gray-900 dark:text-gray-300 dark:hover:text-white px-3 py-2 rounded-md text-sm font-medium"
  >
    {{ t('logout') }}
  </button>
</template>

<script setup>
const { t } = useTranslations()
const { logout } = useAuth()

const handleLogout = () => {
  logout()
}
</script> 