<template>
  <TransitionRoot appear :show="true" as="template">
    <Dialog as="div" @close="$emit('close')" class="relative z-[80]">
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
            <DialogPanel :dir="dir" class="w-full max-w-lg transform overflow-hidden rounded-2xl bg-gradient-to-br from-white to-gray-50 dark:from-gray-800 dark:to-gray-900 p-6 shadow-[0_0_50px_rgba(79,70,229,0.15)] dark:shadow-[0_0_50px_rgba(79,70,229,0.3)] transition-all">
              <div class="text-center">
                <!-- Animated Header -->
                <div class="relative inline-block mb-4">
                  <div class="absolute inset-0 bg-gradient-to-r from-indigo-500 to-purple-500 rounded-full blur-xl opacity-30 animate-pulse-slow"></div>
                  <div class="relative bg-white dark:bg-gray-800 rounded-full p-3 inline-block">
                    <i class="fas fa-crown text-4xl bg-gradient-to-r from-yellow-400 to-yellow-600 bg-clip-text text-transparent"></i>
                  </div>
                </div>

                <DialogTitle as="h2" class="text-2xl font-bold bg-gradient-to-r from-indigo-500 to-purple-500 bg-clip-text text-transparent mb-2">
                  {{ t('unlockPremium') }}
                </DialogTitle>
                <p class="text-base text-gray-600 dark:text-gray-300 mb-6">{{ t('guestModalDescription') }}</p>

                <!-- Features Grid -->
                <div class="grid grid-cols-2 gap-4 mb-6">
                  <div class="feature-card group">
                    <i class="fas fa-chart-line text-xl text-indigo-500 mb-2 group-hover:scale-110 transition-transform"></i>
                    <h3 class="text-sm font-semibold text-gray-800 dark:text-white">{{ t('progressTracking') }}</h3>
                  </div>
                  
                  <div class="feature-card group">
                    <i class="fas fa-trophy text-xl text-yellow-500 mb-2 group-hover:scale-110 transition-transform"></i>
                    <h3 class="text-sm font-semibold text-gray-800 dark:text-white">{{ t('achievements') }}</h3>
                  </div>
                  
                  <div class="feature-card group">
                    <i class="fas fa-users text-xl text-green-500 mb-2 group-hover:scale-110 transition-transform"></i>
                    <h3 class="text-sm font-semibold text-gray-800 dark:text-white">{{ t('multiplayerMode') }}</h3>
                  </div>
                  
                  <div class="feature-card group">
                    <i class="fas fa-cloud-upload-alt text-xl text-blue-500 mb-2 group-hover:scale-110 transition-transform"></i>
                    <h3 class="text-sm font-semibold text-gray-800 dark:text-white">{{ t('cloudSync') }}</h3>
                  </div>
                </div>

                <!-- Action Buttons -->
                <div class="flex flex-col sm:flex-row justify-center items-center gap-3">
                  <NuxtLink
                    to="/register"
                    class="w-full sm:w-auto bg-gradient-to-r from-indigo-500 to-purple-500 text-white px-6 py-2 rounded-lg hover:from-indigo-600 hover:to-purple-600 transform hover:scale-105 transition-all duration-200 text-sm font-medium"
                  >
                    {{ t('register') }}
                  </NuxtLink>
                  
                  <button
                    @click="$emit('close')"
                    class="w-full sm:w-auto bg-gray-100 dark:bg-gray-700 text-gray-700 dark:text-gray-300 px-6 py-2 rounded-lg hover:bg-gray-200 dark:hover:bg-gray-600 transition-all duration-200 text-sm font-medium"
                  >
                    {{ t('continueFree') }}
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
import { Dialog, DialogPanel, DialogTitle, TransitionChild, TransitionRoot } from '@headlessui/vue'
import { useTranslations } from '~/composables/useTranslations'

const { t, dir } = useTranslations()

defineEmits(['close'])
</script>

<style scoped>
.feature-card {
  @apply bg-white/50 dark:bg-gray-800/50 p-3 rounded-xl text-center transform transition-all duration-300 backdrop-blur-sm hover:bg-gradient-to-br hover:from-indigo-500/5 hover:to-purple-500/5 dark:hover:from-indigo-500/10 dark:hover:to-purple-500/10;
}

.animate-pulse-slow {
  animation: pulse 3s cubic-bezier(0.4, 0, 0.6, 1) infinite;
}

@keyframes pulse {
  0%, 100% {
    opacity: 1;
  }
  50% {
    opacity: .5;
  }
}
</style> 