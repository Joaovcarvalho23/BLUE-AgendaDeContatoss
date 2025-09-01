using Agenda.Application.Services;
using Agenda.Domain.Entidades;
using Agenda.Domain.Interfaces;
using Moq;
using Xunit;

namespace Agenda.Tests.Services
{
    public class ContatoServiceTests
    {
        private readonly Mock<IContatoRepositorio> _mockRepo;
        private readonly ContatoService _service;

        public ContatoServiceTests()
        {
            _mockRepo = new Mock<IContatoRepositorio>();
            _service = new ContatoService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllAsync_DeveRetornarSomenteContatosValidos()
        {
            // Arrange
            var lista = new List<Contato>
            {
                new Contato { Id = 1, Nome = "João", Email = "joao@email.com", Telefone = "1111" },
                new Contato { Id = 2, Nome = "" } // inválido
            };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(lista);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("João", result.First().Nome);
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarNull_QuandoContatoNaoExistir()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Contato?)null);

            var result = await _service.GetByIdAsync(10);

            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_DeveRetornarFalse_QuandoCamposInvalidos()
        {
            var contato = new Contato { Nome = "", Email = "", Telefone = "" };

            var result = await _service.AddAsync(contato);

            Assert.False(result);
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Contato>()), Times.Never);
        }

        [Fact]
        public async Task AddAsync_DeveRetornarTrue_QuandoContatoValido()
        {
            var contato = new Contato { Nome = "Maria", Email = "maria@email.com", Telefone = "2222" };

            var result = await _service.AddAsync(contato);

            Assert.True(result);
            _mockRepo.Verify(r => r.AddAsync(contato), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_DeveRetornarFalse_SeContatoNaoExistir()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Contato?)null);

            var result = await _service.UpdateAsync(1, new Contato { Nome = "Novo", Email = "n@email.com", Telefone = "9999" });

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_DeveRetornarFalse_SeContatoNaoExistir()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Contato?)null);

            var result = await _service.DeleteAsync(1);

            Assert.False(result);
        }
    }
}