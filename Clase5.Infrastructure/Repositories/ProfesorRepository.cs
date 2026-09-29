using Clase2.Application.Abstractions.Entities;
using Clase2.Domain.Entities;
using Clase5.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Clase5.Infrastructure.Repositories;

public sealed class ProfesorRepository(AppDbContext context) : IProfesorRepository
{
    public async Task AddAsync(Profesor profesor, CancellationToken cancellationToken)
    {
        await context.Profesores.AddAsync(profesor, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task<Profesor?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
        => context.Profesores.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public Task<Profesor?> FindByLegajoAsync(string legajo, CancellationToken cancellationToken)
        => context.Profesores.FirstOrDefaultAsync(p => p.Legajo == legajo, cancellationToken);

    public async Task UpdateAsync(Profesor profesor, CancellationToken cancellationToken)
    {
        context.Profesores.Update(profesor);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Profesor profesor, CancellationToken cancellationToken)
    {
        context.Profesores.Remove(profesor);
        await context.SaveChangesAsync(cancellationToken);
    }
}
