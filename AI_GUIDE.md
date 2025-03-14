# راهنمای AI برای پروژه Wordle

## ساختار پروژه
- پروژه از JavaScript استفاده می‌کند (نه TypeScript)
- مسیر اصلی پروژه: `/home/office/projects/WorldleGame`
- فرانت‌اند: `/home/office/projects/WorldleGame/WordleFrontend`
- بک‌اند: `/home/office/projects/WorldleGame/WordleBackend`

## نکات مهم
1. همیشه از JavaScript استفاده شود، نه TypeScript
2. برای اجرای دستورات حتماً در مسیر صحیح باشیم
3. پروژه از Nuxt 3 استفاده می‌کند
4. تنظیمات i18n برای پشتیبانی از فارسی و انگلیسی
5. استفاده از Vuetify برای رابط کاربری
6. پشتیبانی از حالت تاریک/روشن با @nuxtjs/color-mode

## دستورات پرکاربرد
```bash
# اجرای فرانت‌اند
cd /home/office/projects/WorldleGame/WordleFrontend && npm run dev

# اجرای بک‌اند
cd /home/office/projects/WorldleGame/WordleBackend && dotnet run
```

## ویژگی‌های اصلی
- پشتیبانی از RTL برای زبان فارسی
- سیستم احراز هویت با قابلیت بازی به عنوان مهمان
- تم تاریک/روشن
- امتیازات و جدول رده‌بندی
- سیستم راهنمایی برای حدس حروف

## نکات مهم برای توسعه
1. همیشه از `defineNuxtPlugin` برای تعریف پلاگین‌ها استفاده شود
2. تنظیمات تم در `nuxt.config.js` و `tailwind.config.js`
3. مسیریابی به صورت خودکار بر اساس ساختار پوشه `pages`
4. استفاده از `composables` برای منطق مشترک
5. کامپوننت‌های Vuetify برای رابط کاربری 