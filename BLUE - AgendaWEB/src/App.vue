<template>
  <div class="app-container flex flex-column min-h-screen">
    <Navbar />
    <main class="main-content flex-grow-1 p-3">
      <RouterView />
    </main>
    <Toast position="top-right" />
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { useAuthStore } from '@/stores/auth'
import Navbar from '@/components/Navbar.vue'
import Toast from 'primevue/toast'

const authStore = useAuthStore()

onMounted(() => {
  authStore.initialize()
})
</script>

<style>
.app-container {
  min-height: 100vh;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.main-content {
  padding: 1rem;
}

/* Responsividade */
@media (max-width: 768px) {
  .main-content {
    padding: 0.5rem;
  }
}

@media (max-width: 480px) {
  .main-content {
    padding: 0.25rem;
  }
}
</style>
