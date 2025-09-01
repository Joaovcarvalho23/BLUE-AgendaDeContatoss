using Agenda.Application.DTOs.Usuarios;
using FluentValidation;

namespace Agenda.Application.Validations.Usuarios;

public class ValidadorLoginUsuarioDto : AbstractValidator<LoginUsuarioDto>
{
    public ValidadorLoginUsuarioDto()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail é obrigatório.")
            .EmailAddress().WithMessage("E-mail inválido.");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Senha é obrigatória.");
    }
}
