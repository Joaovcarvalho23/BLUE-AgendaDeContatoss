// src/stores/auth.ts
import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { UsuarioResponseDto } from '@/types/auth'
import type { UsuarioResponse } from '@/services/auth'

export const useAuthStore = defineStore('auth', () => {
  //const usuario = ref<UsuarioResponseDto | null>(null)
  const usuario = ref<UsuarioResponse | null>(null) // Alterado para refletir o tipo correto
  const token = ref<string | null>(null)
  const isAuthenticated = ref(false)
  const isInitialized = ref(false)

  const usuarioNome = computed(() => {
    return usuario.value?.nome || ''
  })

  // const login = (userData: UsuarioResponseDto, authToken: string) => {
  const login = (userData: UsuarioResponse, authToken: string) => {
    console.log('=== LOGIN ===')
    console.log('Login - userData recebido:', userData)
    console.log('Login - nome do usuário:', userData.nome)

    usuario.value = userData
    token.value = authToken
    isAuthenticated.value = true

    // Salvar no sessionStorage (será limpo ao fechar o navegador)
    sessionStorage.setItem('usuario', JSON.stringify(userData))
    sessionStorage.setItem('token', authToken)

    console.log('Dados salvos no sessionStorage')
    console.log('Verificação imediata - usuario.value:', usuario.value)
    console.log('Verificação imediata - usuario.value.nome:', usuario.value?.nome)
  }

  const logout = () => {
    usuario.value = null
    token.value = null
    isAuthenticated.value = false
    isInitialized.value = false

    // Remover do sessionStorage
    sessionStorage.removeItem('usuario')
    sessionStorage.removeItem('token')
  }

  const initialize = () => {
    if (isInitialized.value) {
      console.log('Store já foi inicializada')
      return
    }

    console.log('=== INITIALIZE ===')
    console.log('Inicializando auth store...')

    const savedUsuario = sessionStorage.getItem('usuario')
    const savedToken = sessionStorage.getItem('token')

    console.log('Raw savedUsuario:', savedUsuario)
    console.log('Raw savedToken:', savedToken)

    if (savedUsuario && savedToken) {
      try {
        const parsedUsuario = JSON.parse(savedUsuario)
        console.log('✅ Usuário parseado com sucesso:', parsedUsuario)
        console.log('✅ Nome do usuário parseado:', parsedUsuario.nome)

        usuario.value = parsedUsuario
        token.value = savedToken
        isAuthenticated.value = true

        console.log('✅ Estado final da store:', {
          'usuario.value': usuario.value,
          'usuario.value.nome': usuario.value?.nome,
          'isAuthenticated.value': isAuthenticated.value
        })
      } catch (error) {
        console.error('❌ Erro ao fazer parse dos dados do usuário:', error)
        // Limpar dados corrompidos
        sessionStorage.removeItem('usuario')
        sessionStorage.removeItem('token')
      }
    } else {
      console.log('❌ Nenhum dado de sessão encontrado')
    }

    isInitialized.value = true
  }

  return {
    usuario,
    token,
    isAuthenticated,
    isInitialized,
    usuarioNome,
    login,
    logout,
    initialize
  }
})
