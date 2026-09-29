using Clase2.Application.Abstractions.Entities;
using MediatR;

namespace Clase2.Application.Commands;

public sealed record ActualizarCursoCommand(Guid Id, string Name, int Credits) : IRequest<bool>;

public sealed class ActualizarCursoCommandHandler(ICursoRepository repository)
    : IRequestHandler<ActualizarCursoCommand, bool>
{
    public async Task<bool> Handle(ActualizarCursoCommand request, CancellationToken cancellationToken)
    {
        var curso = await repository.FindByIdAsync(request.Id, cancellationToken);
        if (curso is null)
            return false;

        curso.UpdateDetails(request.Name, request.Credits);
        await repository.UpdateAsync(curso, cancellationToken);
        return true;
    }
}
