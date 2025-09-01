import api from './api'
import { ErrorHandler } from './errorHandler'

export interface LoginRequest {
  email: string
  senha: string
}

export interface RegisterRequest {
  nome: string
  email: string
  telefone: string
  cpf: string
  senha: string
  confirmarSenha: string
}

export interface UsuarioResponse {
  id: number
  nome: string
  email: string
  telefone: string
  cpf: string
}

export const authService = {
  async login(credentials: LoginRequest): Promise<UsuarioResponse> {
    try {
      const response = await api.post('/Usuarios/login-usuario', credentials)
      return response.data
    } catch (error) {
      throw ErrorHandler.handle(error, 'Erro ao fazer login')
    }
  },

  async register(userData: RegisterRequest): Promise<UsuarioResponse> {
    try {
      const response = await api.post('/Usuarios/registrar-usuario', userData)
      return response.data
    } catch (error) {
      throw ErrorHandler.handle(error, 'Erro ao registrar usuário')
    }
  },
}
