using Agenda.Application.DTOs.Usuarios;
using Agenda.Application.Utils;
using FluentValidation;

namespace Agenda.Application.Validations.Usuarios;

public class ValidadorRegistroUsuarioDto : AbstractValidator<RegistrarUsuarioDto>
{
    public ValidadorRegistroUsuarioDto()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail é obrigatório.")
            .EmailAddress().WithMessage("E-mail inválido.");

        RuleFor(x => x.Telefone)
            .NotEmpty().WithMessage("Telefone é obrigatório.")
            .Matches(@"^\d{10,11}$").WithMessage("Telefone deve ter 10 ou 11 dígitos.");

        RuleFor(x => x.CPF)
            .NotEmpty().WithMessage("CPF é obrigatório.")
            .Must(CpfUtils.ValidarCpf).WithMessage("CPF inválido.");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .Must(SenhaUtils.SenhaValida)
            .WithMessage("A senha deve ter no mínimo 6 caracteres, com ao menos 1 maiúscula, 1 número e 1 caractere especial.");

        RuleFor(x => x.ConfirmarSenha)
            .Equal(x => x.Senha).WithMessage("As senhas não conferem.");
    }
}
