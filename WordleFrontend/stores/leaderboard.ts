import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import axios from 'axios';

export interface LeaderboardEntry {
  userId: number;
  username: string;
  score: number;
  rank: number;
  gamesPlayed: number;
  winRate: number;
  averageAttempts: number;
  lastPlayed: string;
}

export interface LeaderboardFilter {
  period: 'daily' | 'weekly' | 'monthly' | 'all-time';
  page: number;
  limit: number;
  search?: string;
}

export const useLeaderboardStore = defineStore('leaderboard', () => {
  const entries = ref<LeaderboardEntry[]>([]);
  const loading = ref(false);
  const error = ref<string | null>(null);
  const totalEntries = ref(0);
  const currentFilter = ref<LeaderboardFilter>({
    period: 'daily',
    page: 1,
    limit: 10
  });

  const currentPage = computed(() => currentFilter.value.page);
  const totalPages = computed(() => Math.ceil(totalEntries.value / currentFilter.value.limit));

  async function fetchLeaderboard() {
    try {
      loading.value = true;
      error.value = null;

      const { period, page, limit, search } = currentFilter.value;
      const response = await axios.get(`/api/leaderboard/${period}`, {
        params: {
          page,
          limit,
          search
        }
      });

      entries.value = response.data.entries;
      totalEntries.value = response.data.total;
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'خطا در دریافت جدول رتبه‌بندی';
    } finally {
      loading.value = false;
    }
  }

  function updateFilter(filter: Partial<LeaderboardFilter>) {
    currentFilter.value = {
      ...currentFilter.value,
      ...filter,
      page: filter.period !== currentFilter.value.period ? 1 : filter.page ?? currentFilter.value.page
    };
    fetchLeaderboard();
  }

  function nextPage() {
    if (currentPage.value < totalPages.value) {
      updateFilter({ page: currentPage.value + 1 });
    }
  }

  function previousPage() {
    if (currentPage.value > 1) {
      updateFilter({ page: currentPage.value - 1 });
    }
  }

  function goToPage(page: number) {
    if (page >= 1 && page <= totalPages.value) {
      updateFilter({ page });
    }
  }

  return {
    entries,
    loading,
    error,
    totalEntries,
    currentFilter,
    currentPage,
    totalPages,
    fetchLeaderboard,
    updateFilter,
    nextPage,
    previousPage,
    goToPage
  };
}); 