using Clase2.Application.Abstractions.Entities;
using Clase2.Application.DTOs;
using MediatR;

namespace Clase2.Application.Queries;

public sealed record ObtenerProfesorPorIdQuery(Guid Id) : IRequest<ProfesorDto?>;

public sealed class ObtenerProfesorPorIdQueryHandler(IProfesorRepository repository)
    : IRequestHandler<ObtenerProfesorPorIdQuery, ProfesorDto?>
{
    public async Task<ProfesorDto?> Handle(
        ObtenerProfesorPorIdQuery request, CancellationToken cancellationToken)
    {
        var profesor = await repository.FindByIdAsync(request.Id, cancellationToken);
        return profesor is null
            ? null
            : new ProfesorDto(profesor.Id, profesor.Nombre, profesor.Apellido, profesor.Legajo);
    }
}
