using Agenda.Application.DTOs.Usuarios;
using Agenda.Application.Utils;
using Agenda.Domain.Entidades;
using Agenda.Domain.Interfaces;

namespace Agenda.Application.Services;

public class UsuarioService
{
    private readonly IUsuarioRepositorio _usuarioRepositorio;

    public UsuarioService(IUsuarioRepositorio usuarioRepositorio)
    {
        _usuarioRepositorio = usuarioRepositorio;
    }

    public async Task<UsuarioResponseDto> RegistrarAsync(RegistrarUsuarioDto dto)
    {
        try
        {
            if (!CpfUtils.ValidarCpf(dto.CPF))
                throw new ArgumentException("CPF inválido.");

            if (!SenhaUtils.SenhaValida(dto.Senha))
                throw new ArgumentException("Senha não atende os requisitos mínimos.");

            if (dto.Senha != dto.ConfirmarSenha)
                throw new ArgumentException("As senhas não conferem.");

            //unicidade de e-mail
            var existente = await _usuarioRepositorio.GetByEmailAsync(dto.Email);
            if (existente != null)
                throw new InvalidOperationException("E-mail já cadastrado.");

            //senha hash
            var hash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Telefone = dto.Telefone,
                CPF = new string(dto.CPF.Where(char.IsDigit).ToArray()),
                SenhaHash = hash
            };

            await _usuarioRepositorio.AddAsync(usuario);

            return new UsuarioResponseDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Telefone = usuario.Telefone,
                CPF = usuario.CPF
            };
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Falha ao registrar usuário.", ex);
        }
    }

    public async Task<UsuarioResponseDto?> LoginAsync(LoginUsuarioDto dto)
    {
        try
        {
            var usuario = await _usuarioRepositorio.GetByEmailAsync(dto.Email);
            if (usuario == null) return null;

            var ok = BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash);
            if (!ok) return null;

            return new UsuarioResponseDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Telefone = usuario.Telefone,
                CPF = usuario.CPF
            };
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Falha ao realizar login.", ex);
        }
    }
}
