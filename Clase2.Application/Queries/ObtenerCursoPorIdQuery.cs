using Clase2.Application.Abstractions.Entities;
using Clase2.Domain.Entities;
using MediatR;

namespace Clase2.Application.Queries;

public sealed record ObtenerCursoPorIdQuery(Guid Id) : IRequest<Curso?>;

public sealed class ObtenerCursoPorIdQueryHandler(ICursoRepository repository)
    : IRequestHandler<ObtenerCursoPorIdQuery, Curso?>
{
    public async Task<Curso?> Handle(ObtenerCursoPorIdQuery request, CancellationToken cancellationToken)
    {
        return await repository.FindByIdAsync(request.Id, cancellationToken);
    }
}
