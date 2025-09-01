using Agenda.Domain.Entidades;
using Agenda.Domain.Interfaces;
using Agenda.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Infrastructure.Repositorios
{
    public class ContatoRepositorio : IContatoRepositorio
    {
        private readonly AgendaContext _context;

        public ContatoRepositorio(AgendaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Contato>> GetAllAsync()
        {
            try
            {
                return await _context.Contatos.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro no banco ao buscar todos os contatos: ", ex);
            }
        }

        public async Task<Contato?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Contatos.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro no banco ao buscar o contato informado: ", ex);
            }
        }

        public async Task AddAsync(Contato contato)
        {
            try
            {
                _context.Contatos.Add(contato);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro no banco ao adicionar o contato: ", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var contato = await GetByIdAsync(id);

                if (contato != null)
                {
                    _context.Contatos.Remove(contato);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro no banco ao deletar o contato: ", ex);
            }
        }

        public async Task UpdateAsync(Contato contato)
        {
            try
            {
                _context.Contatos.Update(contato);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro no banco ao editar o contato: ", ex);
            }
        }
    }
}
