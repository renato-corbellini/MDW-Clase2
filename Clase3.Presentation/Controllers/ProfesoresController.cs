using System.ComponentModel.DataAnnotations;
using Clase2.Application.Commands;
using Clase2.Application.DTOs;
using Clase2.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clase3.Presentation.Controllers;

[ApiController]
[Route("api/profesores")]
public sealed class ProfesoresController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] ProfesorRequest request, CancellationToken ct)
    {
        var id = await mediator.Send(
            new CrearProfesorCommand(request.Nombre, request.Apellido, request.Legajo), ct);

        if (id is null)
            return Conflict(new ProblemDetails { Title = "Ya existe un profesor con ese legajo." });

        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProfesorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var profesor = await mediator.Send(new ObtenerProfesorPorIdQuery(id), ct);
        return profesor is null ? NotFound() : Ok(profesor);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProfesorRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(
            new ActualizarProfesorCommand(id, request.Nombre, request.Apellido, request.Legajo), ct);

        return result switch
        {
            ActualizarProfesorResult.Updated => NoContent(),
            ActualizarProfesorResult.NotFound => NotFound(),
            ActualizarProfesorResult.LegajoDuplicado =>
                Conflict(new ProblemDetails { Title = "Ya existe un profesor con ese legajo." }),
            _ => throw new InvalidOperationException($"Resultado de actualización inesperado: {result}.")
        };
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var deleted = await mediator.Send(new EliminarProfesorCommand(id), ct);
        return deleted ? NoContent() : NotFound();
    }
}

public sealed class ProfesorRequest
{
    [Required]
    [StringLength(100)]
    public string Nombre { get; init; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Apellido { get; init; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Legajo { get; init; } = string.Empty;
}
