using Clase2.Application.Abstractions.Entities;
using Clase2.Domain.Entities;
using Clase5.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Clase5.Infrastructure.Repositories;

public sealed class CursoRepository(AppDbContext context) : ICursoRepository
{
    public Task<Curso?> FindByIdAsync(Guid id, CancellationToken cancellationToken) 
        => context.Cursos.FirstOrDefaultAsync((c => c.Id == id), cancellationToken);

    public async Task AddAsync(Curso curso, CancellationToken cancellationToken)
    {
        await context.Cursos.AddAsync(curso, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<(IReadOnlyList<Curso> items, int TotalCount)> ListPaginatedAsync(int pageNumber, int pageSize,
        CancellationToken cancellationToken)
    {
        var query = context.Cursos.AsNoTracking().OrderBy(c => c.Name);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((pageNumber - 1) + pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, totalCount);
    }

    public async Task UpdateAsync(Curso curso, CancellationToken cancellationToken)
    {
        context.Cursos.Update(curso);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Curso curso, CancellationToken cancellationToken)
    {
        context.Cursos.Remove(curso);
        await context.SaveChangesAsync(cancellationToken);
    }
}