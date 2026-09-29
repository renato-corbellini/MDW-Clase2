using Clase2.Application.Abstractions.Entities;
using MediatR;

namespace Clase2.Application.Commands;

public sealed record EliminarProfesorCommand(Guid Id) : IRequest<bool>;

public sealed class EliminarProfesorCommandHandler(IProfesorRepository repository)
    : IRequestHandler<EliminarProfesorCommand, bool>
{
    public async Task<bool> Handle(EliminarProfesorCommand request, CancellationToken cancellationToken)
    {
        var profesor = await repository.FindByIdAsync(request.Id, cancellationToken);
        if (profesor is null)
            return false;

        await repository.DeleteAsync(profesor, cancellationToken);
        return true;
    }
}
