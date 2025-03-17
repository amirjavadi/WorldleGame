import { useTranslations } from '~/composables/useTranslations'

<template>
  <TransitionGroup
    enter-active-class="transition-all duration-500 ease-out"
    enter-from-class="opacity-0 -translate-y-4 scale-95"
    enter-to-class="opacity-100 translate-y-0 scale-100"
    leave-active-class="transition-all duration-300 ease-in"
    leave-from-class="opacity-100 translate-y-0 scale-100"
    leave-to-class="opacity-0 -translate-y-4 scale-95"
    class="space-y-3"
  >
    <div
      v-for="(player, index) in data"
      :key="player.username"
      class="transform transition-all duration-300 hover:scale-102 hover:shadow-lg"
    >
      <div
        class="flex items-center justify-between p-5 rounded-xl backdrop-blur-sm"
        :class="[
          index === 0 ? 'bg-gradient-to-r from-yellow-100 to-amber-100 dark:from-yellow-900/30 dark:to-amber-900/30 shadow-yellow-200/50 dark:shadow-yellow-900/20' :
          index === 1 ? 'bg-gradient-to-r from-gray-100 to-slate-100 dark:from-gray-800/30 dark:to-slate-800/30 shadow-gray-200/50 dark:shadow-gray-900/20' :
          index === 2 ? 'bg-gradient-to-r from-orange-100 to-rose-100 dark:from-orange-900/30 dark:to-rose-900/30 shadow-orange-200/50 dark:shadow-orange-900/20' :
          'bg-gradient-to-r from-indigo-50 to-purple-50 dark:from-indigo-900/10 dark:to-purple-900/10 shadow-indigo-200/30 dark:shadow-indigo-900/10'
        ]"
      >
        <div class="flex items-center space-x-6 rtl:space-x-reverse">
          <div class="relative">
            <div
              class="absolute inset-0 rounded-full animate-ping"
              :class="[
                index === 0 ? 'bg-yellow-400/30' :
                index === 1 ? 'bg-gray-400/30' :
                index === 2 ? 'bg-orange-400/30' :
                'bg-indigo-400/20'
              ]"
            ></div>
            <div
              class="relative w-12 h-12 flex items-center justify-center rounded-full text-lg font-bold transform transition-transform hover:scale-110"
              :class="[
                index === 0 ? 'bg-gradient-to-br from-yellow-300 to-amber-400 text-yellow-900 shadow-lg shadow-yellow-300/50' :
                index === 1 ? 'bg-gradient-to-br from-gray-300 to-slate-400 text-gray-900 shadow-lg shadow-gray-300/50' :
                index === 2 ? 'bg-gradient-to-br from-orange-300 to-rose-400 text-orange-900 shadow-lg shadow-orange-300/50' :
                'bg-gradient-to-br from-indigo-300 to-purple-400 text-indigo-900 shadow-lg shadow-indigo-300/50'
              ]"
            >
              {{ index + 1 }}
            </div>
          </div>
          <div class="flex flex-col">
            <span class="font-bold text-lg text-gray-900 dark:text-white">
              {{ player.username }}
            </span>
            <span class="text-sm text-gray-600 dark:text-gray-400">
              {{ t('gamesPlayed') }}: {{ player.gamesPlayed || 0 }}
            </span>
          </div>
        </div>

        <div class="flex items-center space-x-8 rtl:space-x-reverse">
          <div class="text-center group">
            <div class="text-sm font-medium text-gray-500 dark:text-gray-400 group-hover:text-indigo-600 dark:group-hover:text-indigo-400 transition-colors">
              {{ t('score') }}
            </div>
            <div class="flex items-center space-x-1 rtl:space-x-reverse">
              <i class="fas fa-star text-yellow-500 animate-pulse"></i>
              <div class="text-xl font-bold bg-gradient-to-r from-indigo-600 to-purple-600 dark:from-indigo-400 dark:to-purple-400 bg-clip-text text-transparent">
                {{ player.score }}
              </div>
            </div>
          </div>

          <div class="text-center group">
            <div class="text-sm font-medium text-gray-500 dark:text-gray-400 group-hover:text-green-600 dark:group-hover:text-green-400 transition-colors">
              {{ t('winRate') }}
            </div>
            <div class="flex items-center space-x-1 rtl:space-x-reverse">
              <i class="fas fa-trophy text-green-500 animate-bounce"></i>
              <div class="text-xl font-bold bg-gradient-to-r from-green-600 to-emerald-600 dark:from-green-400 dark:to-emerald-400 bg-clip-text text-transparent">
                {{ player.winRate }}%
              </div>
            </div>
          </div>

          <div class="text-center group">
            <div class="text-sm font-medium text-gray-500 dark:text-gray-400 group-hover:text-blue-600 dark:group-hover:text-blue-400 transition-colors">
              {{ t('bestStreak') }}
            </div>
            <div class="flex items-center space-x-1 rtl:space-x-reverse">
              <i class="fas fa-fire text-orange-500 animate-pulse"></i>
              <div class="text-xl font-bold bg-gradient-to-r from-blue-600 to-cyan-600 dark:from-blue-400 dark:to-cyan-400 bg-clip-text text-transparent">
                {{ player.bestStreak || 0 }}
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </TransitionGroup>
</template>

<script setup>
const props = defineProps({
  data: {
    type: Array,
    required: true,
    default: () => []
  }
})

const { t } = useTranslations()
</script>

<style scoped>
.scale-102 {
  --tw-scale-x: 1.02;
  --tw-scale-y: 1.02;
}

@keyframes float {
  0% {
    transform: translateY(0px);
  }
  50% {
    transform: translateY(-5px);
  }
  100% {
    transform: translateY(0px);
  }
}

.animate-float {
  animation: float 3s ease-in-out infinite;
}
</style> 