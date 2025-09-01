using Agenda.Domain.Entidades;
using Agenda.Infrastructure.Data;
using Agenda.Infrastructure.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Tests.Repositorios
{
    public class ContatoRepositorioTests
    {
        private AgendaContext CriarContextoInMemory()
        {
            var options = new DbContextOptionsBuilder<AgendaContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AgendaContext(options);
        }

        [Fact]
        public async Task AddAsync_DeveAdicionarContato()
        {
            using var context = CriarContextoInMemory();
            var repo = new ContatoRepositorio(context);

            var contato = new Contato { Nome = "João", Email = "joao@email.com", Telefone = "1111" };

            await repo.AddAsync(contato);

            var contatos = await context.Contatos.ToListAsync();
            Assert.Single(contatos);
            Assert.Equal("João", contatos.First().Nome);
        }

        [Fact]
        public async Task GetAllAsync_DeveRetornarTodosContatos()
        {
            using var context = CriarContextoInMemory();
            context.Contatos.AddRange(
                new Contato { Nome = "Maria", Email = "maria@email.com", Telefone = "2222" },
                new Contato { Nome = "Pedro", Email = "pedro@email.com", Telefone = "3333" }
            );
            await context.SaveChangesAsync();

            var repo = new ContatoRepositorio(context);
            var result = await repo.GetAllAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarContato()
        {
            using var context = CriarContextoInMemory();
            var contato = new Contato { Nome = "Carlos", Email = "carlos@email.com", Telefone = "4444" };
            context.Contatos.Add(contato);
            await context.SaveChangesAsync();

            var repo = new ContatoRepositorio(context);
            var result = await repo.GetByIdAsync(contato.Id);

            Assert.NotNull(result);
            Assert.Equal("Carlos", result.Nome);
        }

        [Fact]
        public async Task UpdateAsync_DeveAtualizarContato()
        {
            using var context = CriarContextoInMemory();
            var contato = new Contato { Nome = "Antigo", Email = "a@email.com", Telefone = "5555" };
            context.Contatos.Add(contato);
            await context.SaveChangesAsync();

            var repo = new ContatoRepositorio(context);
            contato.Nome = "Atualizado";
            await repo.UpdateAsync(contato);

            var atualizado = await context.Contatos.FindAsync(contato.Id);
            Assert.Equal("Atualizado", atualizado!.Nome);
        }

        [Fact]
        public async Task DeleteAsync_DeveExcluirContato()
        {
            using var context = CriarContextoInMemory();
            var contato = new Contato { Nome = "Excluir", Email = "e@email.com", Telefone = "6666" };
            context.Contatos.Add(contato);
            await context.SaveChangesAsync();

            var repo = new ContatoRepositorio(context);
            await repo.DeleteAsync(contato.Id);

            var contatos = await context.Contatos.ToListAsync();
            Assert.Empty(contatos);
        }
    }
}