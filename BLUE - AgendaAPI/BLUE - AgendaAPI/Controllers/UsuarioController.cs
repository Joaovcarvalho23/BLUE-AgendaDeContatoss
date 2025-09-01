using Agenda.Application.DTOs.Usuarios;
using Agenda.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Agenda.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly UsuarioService _usuarioService;

    public UsuariosController(UsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpPost("registrar-usuario")]
    public async Task<IActionResult> RegistrarUsuario([FromBody] RegistrarUsuarioDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        try
        {
            var user = await _usuarioService.RegistrarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = user.Id }, user);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ApplicationException ex)
        {
            return Problem(title: "Erro ao registrar", detail: ex.InnerException?.Message ?? ex.Message, statusCode: 500);
        }
    }

    [HttpGet("{id:int}")]
    public IActionResult ObterPorId(int id)
    {
        return NoContent();
    }

    [HttpPost("login-usuario")]
    public async Task<IActionResult> Login([FromBody] LoginUsuarioDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        try
        {
            var user = await _usuarioService.LoginAsync(dto);
            if (user == null)
                return Unauthorized(new { message = "Credenciais inválidas." });

            return Ok(new { message = "Login realizado com sucesso!", usuario = user });
        }
        catch (ApplicationException ex)
        {
            return Problem(title: "Erro no login", detail: ex.InnerException?.Message ?? ex.Message, statusCode: 500);
        }
    }
}
