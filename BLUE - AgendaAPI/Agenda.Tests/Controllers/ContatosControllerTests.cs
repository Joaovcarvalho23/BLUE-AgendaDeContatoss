using Agenda.API.Controllers;
using Agenda.Application.Services;
using Agenda.Domain.Entidades;
using Agenda.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Agenda.Tests.Controllers
{
    public class ContatosControllerTests
    {
        private readonly Mock<IContatoRepositorio> _repositorioMock;
        private readonly ContatoService _contatoService;
        private readonly ContatosController _controller;

        public ContatosControllerTests()
        {
            _repositorioMock = new Mock<IContatoRepositorio>();
            _contatoService = new ContatoService(_repositorioMock.Object);
            _controller = new ContatosController(_contatoService);
        }

        [Fact]
        public async Task BuscarContatoPorId_DeveRetornarNotFound_SeNaoExistir()
        {
            // Arrange
            _repositorioMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                            .ReturnsAsync((Contato?)null);

            // Act
            var result = await _controller.BuscarContatoPorId(1);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFound.StatusCode);
        }
    }

}