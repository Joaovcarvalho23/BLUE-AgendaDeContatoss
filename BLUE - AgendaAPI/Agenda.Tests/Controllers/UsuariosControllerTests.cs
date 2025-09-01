using Agenda.API.Controllers;
using Agenda.Application.DTOs.Usuarios;
using Agenda.Application.Services;
using Agenda.Domain.Entidades;
using Agenda.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Agenda.Tests.Controllers;

public class UsuariosControllerTests
{
    private readonly Mock<IUsuarioRepositorio> _usuarioRepositorioMock;
    private readonly UsuarioService _usuarioService;
    private readonly UsuariosController _controller;

    public UsuariosControllerTests()
    {
        _usuarioRepositorioMock = new Mock<IUsuarioRepositorio>();
        _usuarioService = new UsuarioService(_usuarioRepositorioMock.Object);
        _controller = new UsuariosController(_usuarioService);
    }

    [Fact]
    public async Task RegistrarUsuario_DeveRetornarCreated_SeSucesso()
    {
        // Arrange
        var dto = new RegistrarUsuarioDto
        {
            Nome = "João Teste",
            Email = "joao@email.com",
            Telefone = "11999999999",
            CPF = "12345678909",
            Senha = "Senha@123",
            ConfirmarSenha = "Senha@123"
        };

        _usuarioRepositorioMock.Setup(r => r.GetByEmailAsync(dto.Email))
                               .ReturnsAsync((Usuario?)null);

        _usuarioRepositorioMock.Setup(r => r.AddAsync(It.IsAny<Usuario>()))
                               .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.RegistrarUsuario(dto);

        // Assert
        var created = Assert.IsType<CreatedAtActionResult>(result);
        var response = Assert.IsType<UsuarioResponseDto>(created.Value);
        Assert.Equal(dto.Email, response.Email);
    }

    [Fact]
    public async Task RegistrarUsuario_DeveRetornarBadRequest_SeCpfInvalido()
    {
        // Arrange
        var dto = new RegistrarUsuarioDto
        {
            Nome = "Maria",
            Email = "maria@email.com",
            Telefone = "11888888888",
            CPF = "00000000000", // inválido
            Senha = "Senha@123",
            ConfirmarSenha = "Senha@123"
        };

        // Act
        var result = await _controller.RegistrarUsuario(dto);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }

    [Fact]
    public async Task RegistrarUsuario_DeveRetornarConflict_SeEmailDuplicado()
    {
        // Arrange
        var dto = new RegistrarUsuarioDto
        {
            Nome = "Carlos",
            Email = "carlos@email.com",
            Telefone = "11777777777",
            CPF = "98765432100",
            Senha = "Senha@123",
            ConfirmarSenha = "Senha@123"
        };

        _usuarioRepositorioMock.Setup(r => r.GetByEmailAsync(dto.Email))
                               .ReturnsAsync(new Usuario { Email = dto.Email });

        //Act
        var result = await _controller.RegistrarUsuario(dto);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }

    [Fact]
    public async Task Login_DeveRetornarOk_SeCredenciaisCorretas()
    {
        // Arrange
        var senha = "Senha@123";
        var hash = BCrypt.Net.BCrypt.HashPassword(senha);

        var usuario = new Usuario
        {
            Id = 1,
            Nome = "Teste Login",
            Email = "login@email.com",
            CPF = "12345678909",
            Telefone = "11911111111",
            SenhaHash = hash
        };

        _usuarioRepositorioMock.Setup(r => r.GetByEmailAsync(usuario.Email))
                               .ReturnsAsync(usuario);

        var dto = new LoginUsuarioDto { Email = usuario.Email, Senha = senha };

        // Act
        var result = await _controller.Login(dto);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, ok.StatusCode);
    }

    [Fact]
    public async Task Login_DeveRetornarUnauthorized_SeSenhaErrada()
    {
        // Arrange
        var usuario = new Usuario
        {
            Id = 1,
            Nome = "Teste Login",
            Email = "login@email.com",
            CPF = "12345678909",
            Telefone = "11911111111",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("SenhaCorreta")
        };

        _usuarioRepositorioMock.Setup(r => r.GetByEmailAsync(usuario.Email))
                               .ReturnsAsync(usuario);

        var dto = new LoginUsuarioDto { Email = usuario.Email, Senha = "SenhaErrada" };

        // Act
        var result = await _controller.Login(dto);

        // Assert
        var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal(401, unauthorized.StatusCode);
    }
}
