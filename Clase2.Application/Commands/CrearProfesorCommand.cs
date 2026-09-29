using Clase2.Application.Abstractions.Entities;
using Clase2.Domain.Entities;
using MediatR;

namespace Clase2.Application.Commands;

public sealed record CrearProfesorCommand(string Nombre, string Apellido, string Legajo) : IRequest<Guid?>;

public sealed class CrearProfesorCommandHandler(IProfesorRepository repository)
    : IRequestHandler<CrearProfesorCommand, Guid?>
{
    public async Task<Guid?> Handle(CrearProfesorCommand request, CancellationToken cancellationToken)
    {
        var legajo = request.Legajo.Trim();
        if (await repository.FindByLegajoAsync(legajo, cancellationToken) is not null)
            return null;

        var profesor = new Profesor(request.Nombre, request.Apellido, legajo);
        await repository.AddAsync(profesor, cancellationToken);
        return profesor.Id;
    }
}
