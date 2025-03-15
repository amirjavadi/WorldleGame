<template>
  <div class="container mx-auto px-4">
    <h1 class="text-3xl font-bold mb-6 text-center">جدول رتبه‌بندی</h1>
    <Leaderboard />
  </div>
</template>

<script setup>
import Leaderboard from '@/components/Leaderboard/Leaderboard.vue';

definePageMeta({
  layout: 'default',
  middleware: ['auth']
});
</script> 