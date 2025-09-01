<template>
  <div class="home-container">
    <div class="flex justify-content-between align-items-center mb-4 flex-wrap gap-3">
      <h1>Lista de Contatos</h1>
      <div class="flex gap-2 flex-wrap">
        <div class="p-inputgroup">
          <InputText
            v-model="searchTerm"
            placeholder="Buscar por nome..."
            @input="handleSearch"
            class="w-full"
          />
          <Button icon="pi pi-search" class="p-button-outlined" @click="handleSearch" />
        </div>
        <Button label="Criar Contato" icon="pi pi-plus" @click="goToCreateContact" />
      </div>
    </div>

    <div v-if="loading" class="text-center p-5">
      <ProgressSpinner />
      <p>Carregando contatos...</p>
    </div>

    <div
      v-else-if="filteredContatos.length === 0"
      class="text-center p-5 surface-card border-round"
    >
      <i class="pi pi-search" style="font-size: 3rem; color: #9ca3af"></i>
      <h3>{{ searchTerm ? 'Nenhum resultado encontrado' : 'Nenhum contato encontrado' }}</h3>
      <p>
        {{
          searchTerm ? 'Tente buscar com outros termos' : 'Comece adicionando seu primeiro contato!'
        }}
      </p>
      <Button
        :label="searchTerm ? 'Limpar busca' : 'Criar Primeiro Contato'"
        :icon="searchTerm ? 'pi pi-times' : 'pi pi-plus'"
        @click="searchTerm ? clearSearch() : goToCreateContact()"
      />
    </div>

    <DataTable
      v-else
      :value="filteredContatos"
      :paginator="true"
      :rows="10"
      paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
      :rowsPerPageOptions="[5, 10, 20]"
      currentPageReportTemplate="Mostrando {first} de {last} de {totalRecords} contatos"
      class="p-datatable-striped"
    >
      <Column field="nome" header="Nome" :sortable="true"></Column>
      <Column field="email" header="E-mail" :sortable="true"></Column>
      <Column field="telefone" header="Telefone"></Column>
      <Column header="Ações" :exportable="false" style="min-width: 8rem">
        <template #body="slotProps">
          <div class="flex gap-2">
            <Button
              icon="pi pi-pencil"
              class="p-button-rounded p-button-success p-button-text"
              @click="editContact(slotProps.data.id)"
            />
            <Button
              icon="pi pi-trash"
              class="p-button-rounded p-button-danger p-button-text"
              @click="confirmDelete(slotProps.data)"
            />
          </div>
        </template>
      </Column>
    </DataTable>

    <Dialog
      v-model:visible="deleteDialogVisible"
      :style="{ width: '450px' }"
      header="Confirmar Exclusão"
      :modal="true"
    >
      <div class="confirmation-content">
        <i class="pi pi-exclamation-triangle mr-3" style="font-size: 2rem" />
        <span>
          Deseja excluir o contato <strong>{{ contactToDelete?.nome }}</strong
          >?
        </span>
      </div>
      <template #footer>
        <Button
          label="Não"
          icon="pi pi-times"
          class="p-button-text"
          @click="deleteDialogVisible = false"
        />
        <Button label="Sim" icon="pi pi-check" class="p-button-danger" @click="deleteContact" />
      </template>
    </Dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useToast } from 'primevue/usetoast'
import { contatoService } from '@/services/contatos'
import type { Contato } from '@/types/contatos'
import { ErrorHandler } from '@/services/errorHandler'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Button from 'primevue/button'
import Dialog from 'primevue/dialog'
import InputText from 'primevue/inputtext'
import ProgressSpinner from 'primevue/progressspinner'

const router = useRouter()
const toast = useToast()

const contatos = ref<Contato[]>([])
const filteredContatos = ref<Contato[]>([])
const loading = ref(true)
const deleteDialogVisible = ref(false)
const contactToDelete = ref<Contato | null>(null)
const searchTerm = ref('')

const loadContatos = async () => {
  try {
    loading.value = true
    contatos.value = await contatoService.getAll()
    filteredContatos.value = contatos.value
  } catch (error: any) {
    ErrorHandler.handle(error, toast, 'Erro ao carregar contatos')
  } finally {
    loading.value = false
  }
}

const handleSearch = async () => {
  if (!searchTerm.value.trim()) {
    filteredContatos.value = contatos.value
    return
  }

  try {
    loading.value = true
    const resultados = await contatoService.searchByName(searchTerm.value)
    filteredContatos.value = resultados
  } catch (error: any) {
    ErrorHandler.handle(error, toast, 'Erro ao buscar contatos')
  } finally {
    loading.value = false
  }
}

const clearSearch = () => {
  searchTerm.value = ''
  filteredContatos.value = contatos.value
}

const goToCreateContact = () => {
  router.push('/criar-contato')
}

const editContact = (id: number) => {
  router.push(`/editar-contato/${id}`)
}

const confirmDelete = (contact: Contato) => {
  contactToDelete.value = contact
  deleteDialogVisible.value = true
}

const deleteContact = async () => {
  if (!contactToDelete.value?.id) return

  try {
    await contatoService.delete(contactToDelete.value.id)

    toast.add({
      severity: 'success',
      summary: 'Sucesso!',
      detail: 'Contato excluído com sucesso',
      life: 3000,
    })

    // Recarregar a lista
    await loadContatos()
  } catch (error: any) {
    ErrorHandler.handle(error, toast, 'Erro ao excluir contato')
  } finally {
    deleteDialogVisible.value = false
    contactToDelete.value = null
  }
}

onMounted(() => {
  loadContatos()
})
</script>

<style scoped>
.home-container {
  max-width: 1200px;
  margin: 0 auto;
}

.confirmation-content {
  display: flex;
  align-items: center;
  justify-content: center;
}

.p-inputgroup {
  width: 300px;
}

@media (max-width: 768px) {
  .p-inputgroup {
    width: 100%;
  }

  .flex-wrap {
    flex-direction: column;
    align-items: stretch;
  }
}
</style>
