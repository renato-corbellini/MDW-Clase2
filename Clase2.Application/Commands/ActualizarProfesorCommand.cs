using Clase2.Application.Abstractions.Entities;
using MediatR;

namespace Clase2.Application.Commands;

public enum ActualizarProfesorResult
{
    Updated,
    NotFound,
    LegajoDuplicado
}

public sealed record ActualizarProfesorCommand(Guid Id, string Nombre, string Apellido, string Legajo)
    : IRequest<ActualizarProfesorResult>;

public sealed class ActualizarProfesorCommandHandler(IProfesorRepository repository)
    : IRequestHandler<ActualizarProfesorCommand, ActualizarProfesorResult>
{
    public async Task<ActualizarProfesorResult> Handle(
        ActualizarProfesorCommand request, CancellationToken cancellationToken)
    {
        var profesor = await repository.FindByIdAsync(request.Id, cancellationToken);
        if (profesor is null)
            return ActualizarProfesorResult.NotFound;

        var legajo = request.Legajo.Trim();
        var existing = await repository.FindByLegajoAsync(legajo, cancellationToken);
        if (existing is not null && existing.Id != profesor.Id)
            return ActualizarProfesorResult.LegajoDuplicado;

        profesor.UpdateDetails(request.Nombre, request.Apellido, legajo);
        await repository.UpdateAsync(profesor, cancellationToken);
        return ActualizarProfesorResult.Updated;
    }
}
