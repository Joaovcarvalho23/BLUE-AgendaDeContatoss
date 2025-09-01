import type { ToastServiceMethods } from 'primevue/toastservice'

export interface ApiError {
  status: number
  message: string
  field?: string
  type?: 'validation' | 'business' | 'system'
}

export class ErrorHandler {
  private static readonly ERROR_MESSAGES = {
    // Validação
    REQUIRED_FIELD: 'Este campo é obrigatório',
    INVALID_EMAIL: 'E-mail inválido',
    INVALID_CPF: 'CPF inválido',
    INVALID_PHONE: 'Telefone inválido',
    WEAK_PASSWORD: 'Senha deve ter pelo menos 6 caracteres, 1 letra maiúscula, 1 número e 1 caractere especial',
    PASSWORD_MISMATCH: 'Senhas não conferem',

    // Negócio
    EMAIL_EXISTS: 'E-mail já cadastrado',
    CPF_EXISTS: 'CPF já cadastrado',
    INVALID_CREDENTIALS: 'E-mail ou senha incorretos',

    // Sistema
    CONNECTION_ERROR: 'Erro de conexão. Verifique sua internet.',
    SERVER_ERROR: 'Erro interno do servidor',
    UNKNOWN_ERROR: 'Erro inesperado'
  }

  static handle(
    error: any,
    toast: ToastServiceMethods,
    context: 'login' | 'register' | 'general' = 'general'
  ): string {
    console.error('Error details:', error)

    let message = this.ERROR_MESSAGES.UNKNOWN_ERROR
    let severity: 'error' | 'warn' | 'info' = 'error'

    if (error.response) {
      const status = error.response.status
      const data = error.response.data

      message = this.getErrorMessage(status, data, context)
      severity = this.getErrorSeverity(status)
    } else if (error.request) {
      message = this.ERROR_MESSAGES.CONNECTION_ERROR
      severity = 'warn'
    } else {
      message = error.message || this.ERROR_MESSAGES.UNKNOWN_ERROR
    }

    toast.add({
      severity,
      summary: severity === 'error' ? 'Erro' : 'Aviso',
      detail: message,
      life: 5000
    })

    return message
  }

  private static getErrorMessage(status: number, data: any, context: string): string {
    const apiMessage = data?.message || data?.mensagem

    switch (status) {
      case 400:
        if (context === 'login' && this.isCredentialError(apiMessage)) {
          return this.ERROR_MESSAGES.INVALID_CREDENTIALS
        }
        return apiMessage || 'Dados inválidos'

      case 401:
        return context === 'login'
          ? this.ERROR_MESSAGES.INVALID_CREDENTIALS
          : 'Não autorizado'

      case 403:
        return 'Acesso negado'

      case 404:
        return 'Recurso não encontrado'

      case 409:
        if (this.isEmailConflict(apiMessage)) {
          return this.ERROR_MESSAGES.EMAIL_EXISTS
        }
        if (this.isCpfConflict(apiMessage)) {
          return this.ERROR_MESSAGES.CPF_EXISTS
        }
        return apiMessage || 'Dados já existem'

      case 422:
        return apiMessage || 'Dados de entrada inválidos'

      case 500:
        return this.ERROR_MESSAGES.SERVER_ERROR

      default:
        return apiMessage || `Erro ${status}`
    }
  }

  private static getErrorSeverity(status: number): 'error' | 'warn' | 'info' {
    return status >= 500 ? 'error' : 'warn'
  }

  private static isCredentialError(message: string): boolean {
    if (!message) return false
    const msg = message.toLowerCase()
    return msg.includes('credencial') || msg.includes('credential') ||
           msg.includes('senha') || msg.includes('password') ||
           msg.includes('login') || msg.includes('inválido') ||
           msg.includes('invalid') || msg.includes('incorreto')
  }

  private static isEmailConflict(message: string): boolean {
    if (!message) return false
    return message.toLowerCase().includes('email')
  }

  private static isCpfConflict(message: string): boolean {
    if (!message) return false
    return message.toLowerCase().includes('cpf')
  }
}
