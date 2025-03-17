<template>
  <div class="theme-bg-secondary rounded-lg p-4 text-center">
    <h4 class="text-sm theme-text-secondary mb-2">{{ title }}</h4>
    <p class="text-2xl font-bold theme-text-primary">{{ value }}</p>
  </div>
</template>

<script setup>
defineProps({
  title: {
    type: String,
    required: true
  },
  value: {
    type: [Number, String],
    required: true
  }
})
</script>

<script>
export default {
  name: 'StatBox'
}
</script> 