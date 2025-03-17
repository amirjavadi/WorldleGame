import { ref, onMounted, watch, computed } from 'vue'
import { Dialog, DialogPanel, TransitionRoot, TransitionChild, Tab, TabGroup, TabList, TabPanels, TabPanel } from '@headlessui/vue'
import { useLeaderboardStore } from '~/stores/leaderboard'
import { useAuthStore } from '~/stores/auth'
import { useTranslations } from '~/composables/useTranslations'
import LeaderboardList from './LeaderboardList.vue'

<template>
  <TransitionRoot appear :show="isOpen" as="template">
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
            <DialogPanel class="w-full max-w-5xl transform overflow-hidden rounded-2xl bg-gradient-to-br from-white to-indigo-50 dark:from-gray-800 dark:to-indigo-900/20 p-8 shadow-xl transition-all backdrop-blur-lg">
              <div class="relative">
                <button
                  @click="$emit('close')"
                  class="absolute top-0 group"
                  :class="[dir === 'rtl' ? 'left-0' : 'right-0']"
                >
                  <div class="relative p-2">
                    <div class="absolute inset-0 rounded-full bg-red-100 dark:bg-red-900/30 transform transition-transform group-hover:scale-125 duration-300"></div>
                    <i class="fas fa-times text-xl relative z-10 text-red-600 dark:text-red-400 transform transition-transform group-hover:rotate-90 duration-300"></i>
                  </div>
                </button>

                <div class="text-center mb-10">
                  <div class="inline-flex items-center space-x-3 rtl:space-x-reverse">
                    <i class="fas fa-trophy text-4xl text-yellow-500 animate-bounce"></i>
                    <h2 class="text-3xl font-bold bg-gradient-to-r from-indigo-600 to-purple-600 dark:from-indigo-400 dark:to-purple-400 bg-clip-text text-transparent">
                      {{ t('leaderboard') }}
                    </h2>
                  </div>
                </div>

                <div v-if="leaderboardStore.loading" class="flex flex-col items-center justify-center py-12 space-y-4">
                  <div class="relative">
                    <div class="absolute inset-0 rounded-full bg-indigo-200 dark:bg-indigo-700 animate-ping"></div>
                    <div class="relative w-16 h-16 rounded-full border-4 border-indigo-500 border-t-transparent animate-spin"></div>
                  </div>
                  <div class="text-indigo-600 dark:text-indigo-400 animate-pulse">
                    {{ t('loading') }}
                  </div>
                </div>

                <template v-else>
                  <TabGroup v-model="selectedTabIndex" as="div" class="overflow-hidden">
                    <div class="relative">
                      <div class="absolute inset-0 flex items-center" aria-hidden="true">
                        <div class="w-full border-t border-indigo-200 dark:border-indigo-800"></div>
                      </div>
                      <TabList class="relative flex justify-center space-x-4 rtl:space-x-reverse">
                        <Tab v-slot="{ selected }" as="template">
                          <button
                            class="transform transition-all duration-300 px-6 py-3 rounded-xl text-sm font-medium focus:outline-none"
                            :class="[
                              selected
                                ? 'bg-gradient-to-r from-indigo-500 to-purple-500 text-white shadow-lg shadow-indigo-500/30 scale-110'
                                : 'bg-white dark:bg-gray-800 text-gray-600 dark:text-gray-400 hover:text-indigo-600 dark:hover:text-indigo-400'
                            ]"
                          >
                            <div class="flex items-center space-x-2 rtl:space-x-reverse">
                              <i class="fas fa-globe"></i>
                              <span>{{ t('totalGames') }}</span>
                            </div>
                          </button>
                        </Tab>
                        <Tab v-slot="{ selected }" as="template">
                          <button
                            class="transform transition-all duration-300 px-6 py-3 rounded-xl text-sm font-medium focus:outline-none"
                            :class="[
                              selected
                                ? 'bg-gradient-to-r from-green-500 to-emerald-500 text-white shadow-lg shadow-green-500/30 scale-110'
                                : 'bg-white dark:bg-gray-800 text-gray-600 dark:text-gray-400 hover:text-green-600 dark:hover:text-green-400'
                            ]"
                          >
                            <div class="flex items-center space-x-2 rtl:space-x-reverse">
                              <i class="fas fa-calendar-day"></i>
                              <span>{{ t('dailyChallenge') }}</span>
                            </div>
                          </button>
                        </Tab>
                        <Tab v-slot="{ selected }" as="template">
                          <button
                            class="transform transition-all duration-300 px-6 py-3 rounded-xl text-sm font-medium focus:outline-none"
                            :class="[
                              selected
                                ? 'bg-gradient-to-r from-blue-500 to-cyan-500 text-white shadow-lg shadow-blue-500/30 scale-110'
                                : 'bg-white dark:bg-gray-800 text-gray-600 dark:text-gray-400 hover:text-blue-600 dark:hover:text-blue-400'
                            ]"
                          >
                            <div class="flex items-center space-x-2 rtl:space-x-reverse">
                              <i class="fas fa-calendar-week"></i>
                              <span>{{ t('weeklyStats') }}</span>
                            </div>
                          </button>
                        </Tab>
                        <Tab v-slot="{ selected }" as="template">
                          <button
                            class="transform transition-all duration-300 px-6 py-3 rounded-xl text-sm font-medium focus:outline-none"
                            :class="[
                              selected
                                ? 'bg-gradient-to-r from-orange-500 to-red-500 text-white shadow-lg shadow-orange-500/30 scale-110'
                                : 'bg-white dark:bg-gray-800 text-gray-600 dark:text-gray-400 hover:text-orange-600 dark:hover:text-orange-400'
                            ]"
                          >
                            <div class="flex items-center space-x-2 rtl:space-x-reverse">
                              <i class="fas fa-calendar-alt"></i>
                              <span>{{ t('monthlyStats') }}</span>
                            </div>
                          </button>
                        </Tab>
                      </TabList>
                    </div>

                    <TabPanels class="mt-8">
                      <TabPanel>
                        <LeaderboardList :data="leaderboardStore.allTimeLeaderboard" />
                      </TabPanel>
                      <TabPanel>
                        <LeaderboardList :data="leaderboardStore.dailyLeaderboard" />
                      </TabPanel>
                      <TabPanel>
                        <LeaderboardList :data="leaderboardStore.weeklyLeaderboard" />
                      </TabPanel>
                      <TabPanel>
                        <LeaderboardList :data="leaderboardStore.monthlyLeaderboard" />
                      </TabPanel>
                    </TabPanels>
                  </TabGroup>

                  <div class="mt-8 text-center">
                    <div class="inline-flex items-center space-x-2 rtl:space-x-reverse px-4 py-2 rounded-lg bg-white/50 dark:bg-gray-800/50 text-sm text-gray-600 dark:text-gray-400">
                      <i class="fas fa-clock text-indigo-500 animate-spin-slow"></i>
                      <span>{{ t('lastUpdate') }}: {{ formatLastUpdate }}</span>
                    </div>
                  </div>
                </template>

                <!-- Error Message -->
                <div v-if="leaderboardStore.error" class="mt-6">
                  <div class="relative">
                    <div class="absolute inset-0 bg-red-500/10 dark:bg-red-900/30 animate-pulse rounded-lg"></div>
                    <div class="relative p-4 rounded-lg border border-red-200 dark:border-red-800">
                      <div class="flex items-center space-x-3 rtl:space-x-reverse">
                        <i class="fas fa-exclamation-circle text-xl text-red-500"></i>
                        <p class="text-red-600 dark:text-red-400">{{ leaderboardStore.error }}</p>
                      </div>
                    </div>
                  </div>
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
import { ref, onMounted, watch, computed } from 'vue'
import { Dialog, DialogPanel, TransitionRoot, TransitionChild, Tab, TabGroup, TabList, TabPanels, TabPanel } from '@headlessui/vue'
import { useLeaderboardStore } from '~/stores/leaderboard'
import { useAuthStore } from '~/stores/auth'
import { useTranslations } from '~/composables/useTranslations'
import LeaderboardList from './LeaderboardList.vue'

const props = defineProps({
  isOpen: {
    type: Boolean,
    required: true
  }
})

defineEmits(['close'])

const leaderboardStore = useLeaderboardStore()
const authStore = useAuthStore()
const { t, dir } = useTranslations()

const selectedTabIndex = ref(0)

const formatLastUpdate = computed(() => {
  if (!leaderboardStore.lastUpdate) return ''
  return new Date(leaderboardStore.lastUpdate).toLocaleTimeString()
})

watch(() => props.isOpen, async (isOpen) => {
  if (isOpen) {
    if (!authStore.isLoggedIn) {
      leaderboardStore.error = t('pleaseLoginFirst')
      return
    }
    await leaderboardStore.fetchAllTimeLeaderboard()
  }
})

watch(selectedTabIndex, async (newIndex) => {
  if (!authStore.isLoggedIn) {
    leaderboardStore.error = t('pleaseLoginFirst')
    return
  }
  switch (newIndex) {
    case 0:
      await leaderboardStore.fetchAllTimeLeaderboard()
      break
    case 1:
      await leaderboardStore.fetchDailyLeaderboard()
      break
    case 2:
      await leaderboardStore.fetchWeeklyLeaderboard()
      break
    case 3:
      await leaderboardStore.fetchMonthlyLeaderboard()
      break
  }
})

onMounted(async () => {
  if (props.isOpen) {
    if (!authStore.isLoggedIn) {
      leaderboardStore.error = t('pleaseLoginFirst')
      return
    }
    await leaderboardStore.fetchAllTimeLeaderboard()
  }
})
</script> 