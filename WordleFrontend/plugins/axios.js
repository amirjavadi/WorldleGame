import axios from 'axios'

export default defineNuxtPlugin(() => {
  // تنظیم آدرس پایه API
  axios.defaults.baseURL = 'http://localhost:5275'

  // تنظیم headers پیش‌فرض
  axios.defaults.headers.common['Content-Type'] = 'application/json'
  
  // فقط در محیط مرورگر به localStorage دسترسی داشته باشیم
  if (process.client) {
    // بازیابی توکن از localStorage در صورت وجود
    const token = localStorage.getItem('token')
    if (token) {
      axios.defaults.headers.common['Authorization'] = `Bearer ${token}`
    }

    // تنظیم interceptor برای مدیریت خطاها
    axios.interceptors.response.use(
      response => response,
      error => {
        if (error.response?.status === 401) {
          // در صورت خطای 401، کاربر را به صفحه ورود هدایت می‌کنیم
          localStorage.removeItem('token')
          localStorage.removeItem('user')
          navigateTo('/login')
        }
        return Promise.reject(error)
      }
    )
  }
}) 