using Agenda.Domain.Entidades;

namespace Agenda.Domain.Interfaces;

public interface IUsuarioRepositorio
{
    Task<Usuario?> GetByEmailAsync(string email);
    Task AddAsync(Usuario usuario);
}