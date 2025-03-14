<template>
  <div class="min-h-screen bg-gradient-to-br from-gray-50 to-gray-100 dark:from-gray-900 dark:to-gray-800 p-4">
    <div class="max-w-7xl mx-auto">
      <header class="bg-white dark:bg-gray-800 shadow-lg rounded-lg p-6 mb-8">
        <div class="flex justify-between items-center">
          <h1 class="text-3xl font-bold text-gray-900 dark:text-white">
            {{ t('adminPanel') }}
          </h1>
          <div class="flex items-center space-x-4">
            <button
              @click="navigateTo('/')"
              class="px-4 py-2 bg-gray-100 dark:bg-gray-700 rounded-lg hover:bg-gray-200 dark:hover:bg-gray-600 transition-colors duration-200"
            >
              {{ t('backToGame') }}
            </button>
          </div>
        </div>
      </header>

      <div class="grid grid-cols-1 md:grid-cols-2 gap-8">
        <!-- Word Management -->
        <div class="bg-white dark:bg-gray-800 rounded-lg shadow-lg p-6">
          <h2 class="text-2xl font-bold text-gray-900 dark:text-white mb-6">
            {{ t('wordManagement') }}
          </h2>
          
          <!-- Add Word Form -->
          <form @submit.prevent="addWord" class="space-y-4 mb-8">
            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                {{ t('word') }}
              </label>
              <input
                v-model="newWord"
                type="text"
                required
                class="w-full px-4 py-2 rounded-lg border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-indigo-500"
              />
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                {{ t('difficulty') }}
              </label>
              <select
                v-model="wordDifficulty"
                class="w-full px-4 py-2 rounded-lg border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-indigo-500"
              >
                <option value="easy">{{ t('easy') }}</option>
                <option value="medium">{{ t('medium') }}</option>
                <option value="hard">{{ t('hard') }}</option>
              </select>
            </div>
            <button
              type="submit"
              :disabled="loading"
              class="w-full px-4 py-2 bg-indigo-600 hover:bg-indigo-700 text-white rounded-lg transition-colors duration-200"
            >
              {{ loading ? t('adding') : t('addWord') }}
            </button>
          </form>

          <!-- Word List -->
          <div class="space-y-4">
            <h3 class="text-xl font-semibold text-gray-900 dark:text-white mb-4">
              {{ t('wordList') }}
            </h3>
            <div class="space-y-2 max-h-96 overflow-y-auto">
              <div
                v-for="word in words"
                :key="word.id"
                class="flex items-center justify-between p-3 bg-gray-50 dark:bg-gray-700 rounded-lg"
              >
                <span class="text-gray-900 dark:text-white">{{ word.text }}</span>
                <div class="flex items-center space-x-2">
                  <span class="text-sm text-gray-500 dark:text-gray-400">
                    {{ t(word.difficulty) }}
                  </span>
                  <button
                    @click="deleteWord(word.id)"
                    class="text-red-600 hover:text-red-700 dark:text-red-400 dark:hover:text-red-300"
                  >
                    <i class="fas fa-trash"></i>
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Statistics -->
        <div class="bg-white dark:bg-gray-800 rounded-lg shadow-lg p-6">
          <h2 class="text-2xl font-bold text-gray-900 dark:text-white mb-6">
            {{ t('statistics') }}
          </h2>
          
          <div class="grid grid-cols-2 gap-4 mb-8">
            <div class="bg-gray-50 dark:bg-gray-700 p-4 rounded-lg">
              <h4 class="text-sm font-medium text-gray-500 dark:text-gray-400">
                {{ t('totalPlayers') }}
              </h4>
              <p class="text-2xl font-bold text-gray-900 dark:text-white">
                {{ stats.totalPlayers }}
              </p>
            </div>
            <div class="bg-gray-50 dark:bg-gray-700 p-4 rounded-lg">
              <h4 class="text-sm font-medium text-gray-500 dark:text-gray-400">
                {{ t('totalGames') }}
              </h4>
              <p class="text-2xl font-bold text-gray-900 dark:text-white">
                {{ stats.totalGames }}
              </p>
            </div>
            <div class="bg-gray-50 dark:bg-gray-700 p-4 rounded-lg">
              <h4 class="text-sm font-medium text-gray-500 dark:text-gray-400">
                {{ t('averageScore') }}
              </h4>
              <p class="text-2xl font-bold text-gray-900 dark:text-white">
                {{ stats.averageScore }}
              </p>
            </div>
            <div class="bg-gray-50 dark:bg-gray-700 p-4 rounded-lg">
              <h4 class="text-sm font-medium text-gray-500 dark:text-gray-400">
                {{ t('totalWords') }}
              </h4>
              <p class="text-2xl font-bold text-gray-900 dark:text-white">
                {{ stats.totalWords }}
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
definePageMeta({
  middleware: ['admin']
});

import { ref, onMounted } from 'vue';
import { useTranslations } from '~/composables/useTranslations';
import { useApi } from '~/composables/useApi';
import { useAuth } from '~/composables/useAuth';

const { t } = useTranslations();
const api = useApi();
const { isAuthenticated } = useAuth();

const words = ref([]);
const newWord = ref('');
const wordDifficulty = ref('medium');
const loading = ref(false);
const stats = ref({
  totalPlayers: 0,
  totalGames: 0,
  averageScore: 0,
  totalWords: 0,
});

const loadWords = async () => {
  try {
    const response = await api.admin.getWords();
    words.value = response.words;
  } catch (error) {
    console.error('Failed to load words:', error);
  }
};

const loadStats = async () => {
  try {
    const response = await api.admin.getStats();
    stats.value = response;
  } catch (error) {
    console.error('Failed to load stats:', error);
  }
};

const addWord = async () => {
  if (!newWord.value.trim()) return;
  
  loading.value = true;
  try {
    await api.admin.addWord({
      text: newWord.value.trim(),
      difficulty: wordDifficulty.value,
    });
    newWord.value = '';
    await loadWords();
  } catch (error) {
    console.error('Failed to add word:', error);
  } finally {
    loading.value = false;
  }
};

const deleteWord = async (wordId) => {
  try {
    await api.admin.deleteWord(wordId);
    await loadWords();
  } catch (error) {
    console.error('Failed to delete word:', error);
  }
};

onMounted(async () => {
  if (!isAuthenticated.value) {
    navigateTo('/login');
    return;
  }
  
  await Promise.all([
    loadWords(),
    loadStats(),
  ]);
});
</script> 