using Clase2.Domain.Entities;

namespace Clase2.Application.Abstractions.Entities;

public interface ICursoRepository
{
    Task AddAsync(Curso curso, CancellationToken cancellationToken);
    
    Task<Curso?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<(IReadOnlyList<Curso> items, int TotalCount)> ListPaginatedAsync(int pageNumber, int pageSize,
        CancellationToken cancellationToken);
    
    Task UpdateAsync(Curso curso, CancellationToken cancellationToken);
    
    Task DeleteAsync(Curso curso, CancellationToken cancellationToken);
}