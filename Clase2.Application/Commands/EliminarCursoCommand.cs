using Clase2.Application.Abstractions.Entities;
using MediatR;

namespace Clase2.Application.Commands;

public sealed record EliminarCursoCommand(Guid Id) : IRequest<bool>;

public sealed class EliminarCursoCommandHandler(ICursoRepository repository)
    : IRequestHandler<EliminarCursoCommand, bool>
{
    public async Task<bool> Handle(EliminarCursoCommand request, CancellationToken cancellationToken)
    {
        var curso = await repository.FindByIdAsync(request.Id, cancellationToken);
        if (curso is null)
            return false;

        await repository.DeleteAsync(curso, cancellationToken);
        return true;
    }
}
