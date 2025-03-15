<template>
  <div class="flex flex-col md:flex-row gap-4 p-4 bg-white dark:bg-gray-800 rounded-lg shadow">
    <div class="flex-1">
      <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
        دوره زمانی
      </label>
      <select
        v-model="selectedPeriod"
        class="w-full px-3 py-2 bg-white dark:bg-gray-700 border border-gray-300 dark:border-gray-600 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 dark:focus:ring-indigo-400"
      >
        <option value="daily">روزانه</option>
        <option value="weekly">هفتگی</option>
        <option value="monthly">ماهانه</option>
        <option value="all-time">کل</option>
      </select>
    </div>

    <div class="flex-1">
      <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
        جستجو
      </label>
      <div class="relative">
        <input
          v-model="searchQuery"
          type="text"
          placeholder="جستجوی نام کاربری..."
          class="w-full px-3 py-2 bg-white dark:bg-gray-700 border border-gray-300 dark:border-gray-600 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 dark:focus:ring-indigo-400"
        />
        <button
          v-if="searchQuery"
          @click="clearSearch"
          class="absolute inset-y-0 left-0 px-3 flex items-center text-gray-400 hover:text-gray-600 dark:hover:text-gray-200"
        >
          <span class="sr-only">پاک کردن جستجو</span>
          <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
            <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clip-rule="evenodd" />
          </svg>
        </button>
      </div>
    </div>

    <div class="flex-1">
      <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
        تعداد نمایش
      </label>
      <select
        v-model="selectedLimit"
        class="w-full px-3 py-2 bg-white dark:bg-gray-700 border border-gray-300 dark:border-gray-600 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 dark:focus:ring-indigo-400"
      >
        <option :value="10">10</option>
        <option :value="25">25</option>
        <option :value="50">50</option>
        <option :value="100">100</option>
      </select>
    </div>
  </div>
</template>

<script setup>
import { ref, watch } from 'vue';
import { useLeaderboardStore } from '@/stores/leaderboard';

const store = useLeaderboardStore();

const selectedPeriod = ref(store.currentFilter.period);
const searchQuery = ref(store.currentFilter.search || '');
const selectedLimit = ref(store.currentFilter.limit);

watch(selectedPeriod, (newPeriod) => {
  store.updateFilter({ period: newPeriod });
});

watch(searchQuery, (newQuery) => {
  store.updateFilter({ search: newQuery || undefined });
});

watch(selectedLimit, (newLimit) => {
  store.updateFilter({ limit: newLimit });
});

function clearSearch() {
  searchQuery.value = '';
}
</script> 