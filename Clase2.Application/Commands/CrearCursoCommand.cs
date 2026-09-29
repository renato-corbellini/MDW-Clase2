using Clase2.Application.Common;
using MediatR;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Clase2.Application.Commands;

public sealed record CrearCursoCommand(string Name, int Credits) : IRequest<Result<Guid>>;

public sealed class CrearCursoCommandValidator : AbstractValidator<CrearCursoCommand>
{
    public CrearCursoCommandValidator(IStringLocalizer<SharedResources> localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localizer["NameRequired"]);
        
        RuleFor(x => x.Credits)
            .InclusiveBetween(1,12).WithMessage(localizer["InvalidCredits"]);
    }
}

public sealed class SharedResources {}