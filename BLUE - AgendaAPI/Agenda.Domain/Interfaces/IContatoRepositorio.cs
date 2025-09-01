using Agenda.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda.Domain.Interfaces
{
    public interface IContatoRepositorio
    {
        Task<IEnumerable<Contato>> GetAllAsync();
        Task<Contato?> GetByIdAsync(int id);
        Task AddAsync(Contato contato);
        Task DeleteAsync(int id);
        Task UpdateAsync(Contato contato);
    }
}
