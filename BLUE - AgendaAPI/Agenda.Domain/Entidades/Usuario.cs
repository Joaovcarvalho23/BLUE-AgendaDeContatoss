namespace Agenda.Domain.Entidades;

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;   // será o login
    public string Telefone { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty; // nunca exponha a senha em resposta
}