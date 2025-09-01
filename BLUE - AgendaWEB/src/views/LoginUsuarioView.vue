<template>
  <h1>BLUE - Agenda de Contatos</h1>
  <div class="login-wrapper">
    <div class="login-box">
      <h1>Login</h1>

      <div class="form-group">
        <label>Seu e-mail</label>
        <input
          v-model="form.email"
          type="email"
          placeholder="seuemail@email.com"
          class="form-input"
        >
      </div>

      <div class="form-group">
        <label>Sua senha</label>
        <input
          v-model="form.senha"
          type="password"
          placeholder="sua senha"
          class="form-input"
        >
      </div>

      <div class="remember">
        <input
          type="checkbox"
          id="remember"
          v-model="manterLogado"
        >
        <label for="remember">Manter-me logado</label>
      </div>

      <button @click="handleLogin" class="login-btn">
        Logar
      </button>

      <div class="signup-link">
        <span>Ainda não tem conta?</span>
        <router-link to="/cadastro">Cadastre-se</router-link>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { useToast } from 'primevue/usetoast'
import { useAuthStore } from '@/stores/auth'
import { authService } from '@/services/auth'
import { ErrorHandler } from '@/services/errorHandler'

const router = useRouter()
const toast = useToast()
const authStore = useAuthStore()

const loading = ref(false)
const manterLogado = ref(false)
const form = reactive({
  email: '',
  senha: ''
})

const validateForm = (): boolean => {
  if (!form.email) return false
  if (!form.senha) return false
  return true
}

const handleLogin = async () => {
  if (!validateForm()) {
    toast.add({
      severity: 'warn',
      summary: 'Atenção',
      detail: 'Preencha todos os campos',
      life: 3000
    })
    return
  }

  loading.value = true
  try {
    const usuario = await authService.login(form)
    const token = 'fake-jwt-token-' + Date.now()
    authStore.login(usuario, token)
    router.push('/home')
  } catch (error: any) {
    ErrorHandler.handle(error, toast, 'Erro ao fazer login')
    form.senha = ''
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.login-wrapper {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  background: #f8f9fa;
  padding: 20px;
}

.login-box {
  background: white;
  padding: 2rem;
  border-radius: 12px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
  width: 100%;
  max-width: 380px;
  border: 1px solid #e5e7eb;
}

h1 {
  text-align: center;
  color: #111827;
  font-size: 1.875rem;
  font-weight: bold;
  margin-bottom: 1.5rem;
}

.form-group {
  margin-bottom: 1rem;
}

.form-group label {
  display: block;
  font-weight: 500;
  color: #374151;
  margin-bottom: 0.5rem;
}

.form-input {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #d1d5db;
  border-radius: 8px;
  font-size: 1rem;
}

.form-input:focus {
  outline: none;
  border-color: #3b82f6;
  box-shadow: 0 0 0 2px rgba(59, 130, 246, 0.2);
}

.remember {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  margin-bottom: 1rem;
}

.remember label {
  color: #6b7280;
  font-size: 0.875rem;
}

.login-btn {
  width: 100%;
  background: #3b82f6;
  color: white;
  border: none;
  padding: 0.75rem;
  border-radius: 8px;
  font-weight: 600;
  font-size: 1rem;
  cursor: pointer;
  margin-bottom: 1rem;
}

.login-btn:hover {
  background: #2563eb;
}

.signup-link {
  text-align: center;
  padding-top: 1rem;
  border-top: 1px solid #e5e7eb;
}

.signup-link span {
  color: #6b7280;
}

.signup-link a {
  color: #3b82f6;
  text-decoration: none;
  margin-left: 0.5rem;
  font-weight: 500;
}

.signup-link a:hover {
  text-decoration: underline;
}
</style>
