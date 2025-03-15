<template>
  <div class="fixed top-4 right-4 z-50 space-y-2">
    <TransitionGroup name="notification">
      <div
        v-for="notification in notifications"
        :key="notification.id"
        :class="[
          'notification p-4 rounded-lg shadow-lg max-w-md transform transition-all duration-300',
          {
            'bg-green-500 text-white': notification.type === 'success',
            'bg-red-500 text-white': notification.type === 'error',
            'bg-blue-500 text-white': notification.type === 'info',
            'bg-yellow-500 text-white': notification.type === 'warning'
          }
        ]"
        @click="removeNotification(notification.id)"
      >
        <div class="flex items-center">
          <div class="flex-1">{{ notification.message }}</div>
          <button
            class="ml-4 text-white hover:text-gray-200 focus:outline-none"
            @click.stop="removeNotification(notification.id)"
          >
            ×
          </button>
        </div>
      </div>
    </TransitionGroup>
  </div>
</template>

<script setup>
import { useNotification } from '~/composables/useNotification'

const { notifications, removeNotification } = useNotification()
</script>

<style scoped>
.notification-enter-active,
.notification-leave-active {
  transition: all 0.3s ease;
}

.notification-enter-from {
  opacity: 0;
  transform: translateX(30px);
}

.notification-leave-to {
  opacity: 0;
  transform: translateX(30px);
}

.notification {
  cursor: pointer;
}

.notification:hover {
  transform: translateY(-2px);
}
</style> 