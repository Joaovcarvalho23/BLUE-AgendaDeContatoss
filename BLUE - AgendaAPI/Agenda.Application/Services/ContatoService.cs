using Agenda.Domain.Entidades;
using Agenda.Domain.Interfaces;

namespace Agenda.Application.Services
{
    public class ContatoService
    {
        private readonly IContatoRepositorio _contatoRepositorio;

        public ContatoService (IContatoRepositorio contatoRepositorio)
        {
            _contatoRepositorio = contatoRepositorio;
        }

        public async Task<IEnumerable<Contato>> GetAllAsync()
        {
            try
            {
                var response = await _contatoRepositorio.GetAllAsync();
                var contatosValidos = response.Where(c => !string.IsNullOrWhiteSpace(c.Nome));

                return contatosValidos;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao buscar contatos.", ex);
            }
        }

        public async Task<Contato?> GetByIdAsync(int id)
        {
            try
            {
                var response = await _contatoRepositorio.GetByIdAsync(id);

                if (response == null || string.IsNullOrWhiteSpace(response.Nome))
                    return null;

                return response;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao buscar contato por Id.", ex);
            }
        }

        public async Task<bool> AddAsync(Contato contato)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(contato.Nome) ||
                    string.IsNullOrWhiteSpace(contato.Email) ||
                    string.IsNullOrWhiteSpace(contato.Telefone))
                {
                    return false;
                }

                await _contatoRepositorio.AddAsync(contato);
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao adicionar contato.", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var contatoExistente = await _contatoRepositorio.GetByIdAsync(id);
                if (contatoExistente == null)
                    return false;

                await _contatoRepositorio.DeleteAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao excluir contato.", ex);
            }
        }

        public async Task<bool> UpdateAsync(int id, Contato contato)
        {
            try
            {
                var contatoExistente = await _contatoRepositorio.GetByIdAsync(id);
                if (contatoExistente == null)
                    return false;

                if (string.IsNullOrWhiteSpace(contato.Nome) ||
                    string.IsNullOrWhiteSpace(contato.Email) ||
                    string.IsNullOrWhiteSpace(contato.Telefone))
                {
                    return false;
                }

                contatoExistente.Nome = contato.Nome;
                contatoExistente.Email = contato.Email;
                contatoExistente.Telefone = contato.Telefone;

                await _contatoRepositorio.UpdateAsync(contatoExistente);
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao atualizar contato.", ex);
            }
        }

    }
}
