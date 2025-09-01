<template>
  <nav class="navbar">
    <div class="nav-content">
      <div class="nav-brand">
      <h1>BLUE - Agenda de Contatos</h1>
      </div>

      <div class="nav-user" v-if="authStore.isAuthenticated">

        <span class="welcome-text">
          Olá, {{ nomeUsuario || 'Admin' }}!
        </span>
        <button @click="logout" class="logout-btn">
          <i class="pi pi-sign-out"></i>
          <span>Sair</span>
        </button>
      </div>
    </div>
  </nav>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useAuthStore } from '@/stores/auth'
import { useRouter } from 'vue-router'

const authStore = useAuthStore()
const router = useRouter()

const nomeUsuario = computed(() => {
  return authStore.usuarioNome;
});

const logout = () => {
  authStore.logout()
  router.push('/login')
}
</script>

<style scoped>
.navbar {
  background: white;
  border-bottom: 1px solid #e5e7eb;
  padding: 1rem 2rem;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.nav-content {
  display: flex;
  justify-content: space-between;
  align-items: center;
  max-width: 1200px;
  margin: 0 auto;
}

.nav-brand h2 {
  color: #111827;
  font-size: 1.5rem;
  font-weight: bold;
  margin: 0;
}

.nav-user {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.welcome-text {
  color: #374151;
  font-weight: 500;
}

.logout-btn {
  background: #ef4444;
  color: white;
  border: none;
  padding: 0.5rem 1rem;
  border-radius: 6px;
  cursor: pointer;
  font-weight: 500;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  transition: background-color 0.2s;
}

.logout-btn:hover {
  background: #dc2626;
}

/* Responsividade */
@media (max-width: 768px) {
  .navbar {
    padding: 1rem;
  }

  .nav-brand h2 {
    font-size: 1.2rem;
  }

  .welcome-text {
    display: none;
  }

  .logout-btn {
    padding: 0.5rem;
  }

  .logout-btn span {
    display: none;
  }
}
</style>
