<template>
  <div class="register-wrapper">
    <div class="register-box">
      <h1>Criar Conta</h1>
      <p class="subtitle">Preencha seus dados para começar</p>

      <div class="form-grid">
        <div class="form-group">
          <label>Nome <span class="required">*</span></label>
          <input
            v-model="form.nome"
            type="text"
            placeholder="Seu nome completo"
            class="form-input"
            :class="{ 'error': errors.nome }"
          >
          <small class="error-text" v-if="errors.nome">{{ errors.nome }}</small>
        </div>

        <div class="form-group">
          <label>Telefone <span class="required">*</span></label>
          <input
            v-model="form.telefone"
            type="tel"
            placeholder="(11) 99999-9999"
            class="form-input"
            :class="{ 'error': errors.telefone }"
          >
          <small class="error-text" v-if="errors.telefone">{{ errors.telefone }}</small>
        </div>
      </div>

      <div class="form-group">
        <label>E-mail <span class="required">*</span></label>
        <input
          v-model="form.email"
          type="email"
          placeholder="seu@email.com"
          class="form-input"
          :class="{ 'error': errors.email }"
        >
        <small class="error-text" v-if="errors.email">{{ errors.email }}</small>
      </div>

      <div class="form-group">
        <label>CPF <span class="required">*</span></label>
        <input
          v-model="form.cpf"
          type="text"
          placeholder="000.000.000-00"
          class="form-input"
          :class="{ 'error': errors.cpf }"
        >
        <small class="error-text" v-if="errors.cpf">{{ errors.cpf }}</small>
      </div>

      <div class="form-grid">
        <div class="form-group">
          <label>Senha <span class="required">*</span></label>
          <input
            v-model="form.senha"
            type="password"
            placeholder="Mínimo 6 caracteres"
            class="form-input"
            :class="{ 'error': errors.senha }"
          >
          <small class="error-text" v-if="errors.senha">{{ errors.senha }}</small>
        </div>

        <div class="form-group">
          <label>Confirmar Senha <span class="required">*</span></label>
          <input
            v-model="form.confirmarSenha"
            type="password"
            placeholder="Digite novamente"
            class="form-input"
            :class="{ 'error': errors.confirmarSenha }"
          >
          <small class="error-text" v-if="errors.confirmarSenha">{{ errors.confirmarSenha }}</small>
        </div>
      </div>

      <button @click="handleRegister" class="register-btn" :disabled="loading">
        {{ loading ? 'Criando...' : 'Criar Conta' }}
      </button>

      <div class="login-link">
        <span>Já tem uma conta?</span>
        <router-link to="/login">Faça login aqui</router-link>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { useToast } from 'primevue/usetoast'
import { authService } from '@/services/auth'
import { ErrorHandler } from '@/services/errorHandler'
import { Validators } from '@/utils/validators'

const router = useRouter()
const toast = useToast()

const loading = ref(false)
const form = reactive({
  nome: '',
  email: '',
  telefone: '',
  cpf: '',
  senha: '',
  confirmarSenha: ''
})

const errors = reactive({
  nome: '',
  email: '',
  telefone: '',
  cpf: '',
  senha: '',
  confirmarSenha: ''
})

const validateForm = (): boolean => {
  // Limpa erros anteriores
  Object.keys(errors).forEach(key => errors[key as keyof typeof errors] = '')

  if (!Validators.required(form.nome)) errors.nome = 'Nome é obrigatório'

  if (!Validators.required(form.email)) errors.email = 'E-mail é obrigatório'
  else if (!Validators.email(form.email)) errors.email = 'E-mail inválido'

  if (!Validators.required(form.telefone)) errors.telefone = 'Telefone é obrigatório'
  else if (!Validators.phone(form.telefone)) errors.telefone = 'Telefone inválido'

  if (!Validators.required(form.cpf)) errors.cpf = 'CPF é obrigatório'
  else if (!Validators.cpf(form.cpf)) errors.cpf = 'CPF inválido'

  if (!Validators.required(form.senha)) errors.senha = 'Senha é obrigatória'
  else if (!Validators.minLength(form.senha, 6)) errors.senha = 'Senha deve ter pelo menos 6 caracteres'

  if (!Validators.required(form.confirmarSenha)) errors.confirmarSenha = 'Confirmação de senha é obrigatória'
  else if (!Validators.passwordsMatch(form.senha, form.confirmarSenha)) errors.confirmarSenha = 'Senhas não conferem'

  return Object.values(errors).every(error => !error)
}

const handleRegister = async () => {
  if (!validateForm()) {
    toast.add({
      severity: 'warn',
      summary: 'Atenção',
      detail: 'Preencha todos os campos corretamente',
      life: 3000
    })
    return
  }

  loading.value = true

  try {
    await authService.register(form)

    toast.add({
      severity: 'success',
      summary: 'Sucesso!',
      detail: 'Cadastro realizado com sucesso. Faça login para continuar.',
      life: 5000
    })

    router.push('/login')
  } catch (error: any) {
    ErrorHandler.handle(error, toast, 'Erro ao realizar cadastro')
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.register-wrapper {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  background: #f8f9fa;
  padding: 20px;
}

.register-box {
  background: white;
  padding: 2rem;
  border-radius: 12px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
  width: 100%;
  max-width: 480px;
  border: 1px solid #e5e7eb;
}

h1 {
  text-align: center;
  color: #111827;
  font-size: 1.875rem;
  font-weight: bold;
  margin-bottom: 0.5rem;
}

.subtitle {
  text-align: center;
  color: #6b7280;
  margin-bottom: 2rem;
}

.form-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
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

.required {
  color: #ef4444;
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

.form-input.error {
  border-color: #ef4444;
}

.error-text {
  color: #ef4444;
  font-size: 0.875rem;
  margin-top: 0.25rem;
  display: block;
}

.register-btn {
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
  transition: background-color 0.2s;
}

.register-btn:hover:not(:disabled) {
  background: #2563eb;
}

.register-btn:disabled {
  background: #9ca3af;
  cursor: not-allowed;
}

.login-link {
  text-align: center;
  padding-top: 1rem;
  border-top: 1px solid #e5e7eb;
}

.login-link span {
  color: #6b7280;
}

.login-link a {
  color: #3b82f6;
  text-decoration: none;
  margin-left: 0.5rem;
  font-weight: 500;
}

.login-link a:hover {
  text-decoration: underline;
}

/* Responsividade */
@media (max-width: 640px) {
  .form-grid {
    grid-template-columns: 1fr;
  }

  .register-box {
    padding: 1.5rem;
    margin: 1rem;
  }
}
</style>
