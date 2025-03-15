<template>
  <div class="container mx-auto px-4 py-8">
    <div class="max-w-7xl mx-auto">
      <h1 class="text-3xl font-bold text-gray-900 dark:text-gray-100 mb-8">چالش روزانه</h1>
      
      <DailyChallenge />
    </div>
  </div>
</template>

<script setup>
import { onMounted } from 'vue';
import DailyChallenge from '@/components/DailyChallenge/Challenge.vue';

definePageMeta({
  layout: 'default',
  middleware: ['auth']
});
</script> 