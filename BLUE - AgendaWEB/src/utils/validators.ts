export class Validators {
  static required(value: string): boolean {
    return value != null && value.trim().length > 0
  }

  static email(value: string): boolean {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
    return emailRegex.test(value)
  }

  static phone(value: string): boolean {
    // Remove caracteres não numéricos
    const digits = value.replace(/\D/g, '')
    // Aceita 10 ou 11 dígitos (com ou sem DDD)
    return digits.length >= 10 && digits.length <= 11
  }

  static cpf(value: string): boolean {
    // Remove caracteres não numéricos
    const digits = value.replace(/\D/g, '')

    // Verifica se tem 11 dígitos
    if (digits.length !== 11) return false

    // Verifica se não são todos iguais
    if (/^(\d)\1+$/.test(digits)) return false

    // Validação dos dígitos verificadores
    let sum = 0
    for (let i = 0; i < 9; i++) {
      sum += parseInt(digits[i]) * (10 - i)
    }
    let digit1 = (sum * 10) % 11
    if (digit1 === 10) digit1 = 0

    sum = 0
    for (let i = 0; i < 10; i++) {
      sum += parseInt(digits[i]) * (11 - i)
    }
    let digit2 = (sum * 10) % 11
    if (digit2 === 10) digit2 = 0

    return digit1 === parseInt(digits[9]) && digit2 === parseInt(digits[10])
  }

  static password(value: string): boolean {
    if (value.length < 6) return false

    const hasUpperCase = /[A-Z]/.test(value)
    const hasLowerCase = /[a-z]/.test(value)
    const hasNumber = /\d/.test(value)
    const hasSpecialChar = /[!@#$%^&*(),.?":{}|<>]/.test(value)

    return hasUpperCase && hasLowerCase && hasNumber && hasSpecialChar
  }

  static minLength(value: string, length: number): boolean {
    return value.length >= length
  }

  static passwordsMatch(password: string, confirmPassword: string): boolean {
    return password === confirmPassword
  }
}
