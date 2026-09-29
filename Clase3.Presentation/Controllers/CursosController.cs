using Microsoft.AspNetCore.Mvc;
using MediatR;
using Clase2.Application.Commands;
using Clase2.Application.Queries;
using Clase2.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Clase3.Presentation.Controllers;
 
[ApiController]
[Route("api/cursos")]
public sealed class CursosController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CrearCursoCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);

        if (!result.IsSuccess)
        {
            var problem = new ValidationProblemDetails(
                result.Errors.GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()))
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Uno o más campos no son válidos",
                Type = ""
            };
            
            return BadRequest(problem);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Curso), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var curso = await mediator.Send(new ObtenerCursoPorIdQuery(id), ct);

        return curso is null ? NotFound() : Ok(curso);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] ActualizarCursoRequest request, CancellationToken ct)
    {
        var updated = await mediator.Send(
            new ActualizarCursoCommand(id, request.Name, request.Credits), ct);

        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var deleted = await mediator.Send(new EliminarCursoCommand(id), ct);

        return deleted ? NoContent() : NotFound();
    }
}

public sealed record ActualizarCursoRequest(
    [param: Required] string Name,
    [param: Range(1, 12)] int Credits);
