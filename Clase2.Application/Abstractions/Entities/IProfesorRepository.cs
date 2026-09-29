using Clase2.Domain.Entities;

namespace Clase2.Application.Abstractions.Entities;

public interface IProfesorRepository
{
    Task AddAsync(Profesor profesor, CancellationToken cancellationToken);
    Task<Profesor?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Profesor?> FindByLegajoAsync(string legajo, CancellationToken cancellationToken);
    Task UpdateAsync(Profesor profesor, CancellationToken cancellationToken);
    Task DeleteAsync(Profesor profesor, CancellationToken cancellationToken);
}
