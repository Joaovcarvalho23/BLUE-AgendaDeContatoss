<template>
  <div class="criar-contato-container">
    <div class="flex align-items-center mb-4">
      <Button
        icon="pi pi-arrow-left"
        class="p-button-text p-button-secondary mr-3"
        @click="goBack"
      />
      <div>
        <h1 class="m-0 text-900">Novo Contato</h1>
        <p class="m-0 text-500">Adicione um novo contato à sua agenda</p>
      </div>
    </div>

    <Card class="surface-card shadow-2 border-round-xl">
      <template #content>
        <form @submit.prevent="handleCreate" class="flex flex-column gap-4">
          <div class="field">
            <label for="nome" class="block text-900 font-medium mb-2">
              Nome <span class="text-red-500">*</span>
            </label>
            <InputText
              id="nome"
              v-model="form.nome"
              class="w-full"
              placeholder="Nome completo do contato"
              :class="{ 'p-invalid': errors.nome }"
            />
            <small class="p-error" v-if="errors.nome">{{ errors.nome }}</small>
          </div>

          <div class="field">
            <label for="email" class="block text-900 font-medium mb-2">
              E-mail <span class="text-red-500">*</span>
            </label>
            <InputText
              id="email"
              v-model="form.email"
              type="email"
              class="w-full"
              placeholder="email@exemplo.com"
              :class="{ 'p-invalid': errors.email }"
            />
            <small class="p-error" v-if="errors.email">{{ errors.email }}</small>
          </div>

          <div class="field">
            <label for="telefone" class="block text-900 font-medium mb-2">
              Telefone <span class="text-red-500">*</span>
            </label>
            <InputText
              id="telefone"
              v-model="form.telefone"
              class="w-full"
              placeholder="(11) 99999-9999"
              :class="{ 'p-invalid': errors.telefone }"
            />
            <small class="p-error" v-if="errors.telefone">{{ errors.telefone }}</small>
          </div>

          <div class="flex gap-3 justify-content-end pt-3 border-top-1 surface-border">
            <Button
              label="Cancelar"
              icon="pi pi-times"
              class="p-button-outlined p-button-secondary"
              @click="goBack"
            />
            <Button
              type="submit"
              label="Criar Contato"
              icon="pi pi-check"
              :loading="loading"
            />
          </div>
        </form>
      </template>
    </Card>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { useToast } from 'primevue/usetoast'
import { contatoService } from '@/services/contatos'
import { ErrorHandler } from '@/services/errorHandler'
import { Validators } from '@/utils/validators'
import InputText from 'primevue/inputtext'
import Button from 'primevue/button'
import Card from 'primevue/card'

const router = useRouter()
const toast = useToast()

const loading = ref(false)
const form = reactive({
  nome: '',
  email: '',
  telefone: ''
})

const errors = reactive({
  nome: '',
  email: '',
  telefone: ''
})

const validateForm = (): boolean => {
  errors.nome = ''
  errors.email = ''
  errors.telefone = ''

  if (!form.nome.trim()) errors.nome = 'Nome é obrigatório'
  if (!form.email.trim()) errors.email = 'E-mail é obrigatório'
  else if (!Validators.email(form.email)) errors.email = 'E-mail inválido'
  if (!form.telefone.trim()) errors.telefone = 'Telefone é obrigatório'

  return !errors.nome && !errors.email && !errors.telefone
}

const goBack = () => {
  router.push('/home')
}

const handleCreate = async () => {
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
    await contatoService.create(form)

    toast.add({
      severity: 'success',
      summary: 'Sucesso!',
      detail: 'Contato criado com sucesso',
      life: 3000
    })

    router.push('/home')
  } catch (error: any) {
    ErrorHandler.handle(error, toast, 'Erro ao criar contato')
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.criar-contato-container {
  max-width: 600px;
  margin: 0 auto;
}

:deep(.p-card) {
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(10px);
}

.text-red-500 {
  color: #ef4444;
}
</style>
