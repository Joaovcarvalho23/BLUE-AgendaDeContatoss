import api from './api'
import type { Contato } from '@/types/contatos'

export const contatoService = {
  async getAll(): Promise<Contato[]> {
    const response = await api.get('/Contatos/BuscarTodosContatos')
    return response.data
  },

  async getById(id: number): Promise<Contato> {
    const response = await api.get(`/Contatos/${id}`)
    return response.data
  },

  async searchByName(nome: string): Promise<Contato[]> {
    try {
      const response = await api.get('/Contatos/BuscarTodosContatos')
      const contatos = response.data

      // Filtro local - você pode implementar no backend também
      return contatos.filter((contato: Contato) =>
        contato.nome.toLowerCase().includes(nome.toLowerCase())
      )
    } catch (error) {
      throw error
    }
  },

  async create(contato: Omit<Contato, 'id'>): Promise<Contato> {
    const response = await api.post('/Contatos/CriarContato', contato)
    return response.data
  },

  async update(id: number, contato: Contato): Promise<void> {
    await api.put(`/Contatos/${id}`, contato)
  },

  async delete(id: number): Promise<void> {
    await api.delete(`/Contatos/${id}`)
  }
}
