using Clase2.Application.Abstractions.Entities;
using Clase2.Application.Common;
using Clase2.Domain.Entities;
using MediatR;

namespace Clase2.Application.Commands;

public sealed class CrearCursoCommandHadler(ICursoRepository repository) : IRequestHandler<CrearCursoCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CrearCursoCommand request, CancellationToken cancellationToken)
    {
        var curso = new Curso(request.Name, request.Credits);

        await repository.AddAsync(curso, cancellationToken);
        
        return Result<Guid>.Success(curso.Id);
    }
}