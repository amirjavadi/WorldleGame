<template>
  <div class="container mx-auto px-4 py-8">
    <div class="max-w-7xl mx-auto">
      <h1 class="text-3xl font-bold text-gray-900 dark:text-gray-100 mb-8">آمار و اطلاعات</h1>
      
      <div class="space-y-8">
        <StatsOverview />
        <GuessDistribution />
        <RecentGames />
      </div>
    </div>
  </div>
</template>

<script setup>
import { onMounted } from 'vue';
import { useStatsStore } from '@/stores/stats';
import StatsOverview from '@/components/Stats/Overview.vue';
import GuessDistribution from '@/components/Stats/GuessDistribution.vue';
import RecentGames from '@/components/Stats/RecentGames.vue';

const store = useStatsStore();

onMounted(async () => {
  await store.fetchStats();
  await store.fetchLastGames();
});

definePageMeta({
  layout: 'default',
  middleware: ['auth']
});
</script> 