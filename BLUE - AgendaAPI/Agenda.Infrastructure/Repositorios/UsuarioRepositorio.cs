using Agenda.Domain.Entidades;
using Agenda.Domain.Interfaces;
using Agenda.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Infrastructure.Repositorios;

public class UsuarioRepositorio : IUsuarioRepositorio
{
    private readonly AgendaContext _context;

    public UsuarioRepositorio(AgendaContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        try
        {
            return await _context.Usuarios.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro no banco ao buscar usuário por e-mail.", ex);
        }
    }

    public async Task AddAsync(Usuario usuario)
    {
        try
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("Erro ao salvar usuário no banco (possível e-mail/CPF duplicado).", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro inesperado ao salvar usuário.", ex);
        }
    }
}
