using Microsoft.AspNetCore.Mvc;
using Gestion.API.Homework.Domain.Repositories;
using Gestion.API.Homework.Interfaces.REST.Resources;
using Gestion.API.Homework.Interfaces.REST.Transform;
using Gestion.API.Homework.Domain.Model.Commands;
using Gestion.API.Homework.Domain.Model.ValueObjects;
using Gestion.API.Homework.Domain.Services;
using Microsoft.AspNetCore.Authorization;

namespace Gestion.API.Homework.Interfaces.REST;

[Route("api/[controller]")]
[ApiController]
public class HomeworkController : ControllerBase
{
    private readonly IHomeworkRepository _homeworkRepository;
    private readonly IHomeworkCommandService _homeworkCommandService;

    public HomeworkController(IHomeworkRepository homeworkRepository, IHomeworkCommandService homeworkCommandService)
    {
        _homeworkRepository = homeworkRepository;
        _homeworkCommandService = homeworkCommandService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateHomework([FromBody] HomeworkResource homeworkResource)
    {
        var command = new CreateHomeworkCommand
        {
            Nombre = homeworkResource.Nombre,
            Descripcion = homeworkResource.Descripcion,
            Fecha = homeworkResource.Fecha,
            PriorityTask = (PriorityTask)homeworkResource.PriorityTask,
            ImagenBase64 = homeworkResource.ImagenBase64 // Asignar la nueva propiedad
        };

        await _homeworkCommandService.CreateHomeworkTask(command);

        return CreatedAtAction(nameof(CreateHomework), new { id = homeworkResource.Id }, homeworkResource);
    }

    [Authorize]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllHomeworkTasks()
    {
        var tasks = await _homeworkRepository.GetAllAsync();
        var resources = tasks.Select(HomeworkTransform.ToResource);

        return Ok(resources);
    }
    
    [Authorize]
    [HttpGet("paged")]
    public async Task<IActionResult> GetPagedHomeworkTasks([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
    {
        var tasks = await _homeworkRepository.GetPagedAsync(pageNumber, pageSize);
        var totalCount = await _homeworkRepository.GetTotalCountAsync(); 
        var resources = tasks.Select(HomeworkTransform.ToResource);

        return Ok(new { totalCount, items = resources });
    }


    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateHomework(int id, [FromBody] HomeworkResource homeworkResource)
    {
        if (id != homeworkResource.Id)
        {
            return BadRequest("El ID de la tarea no coincide.");
        }

        var command = new UpdateHomeworkCommand
        {
            Id = id,
            Nombre = homeworkResource.Nombre,
            Descripcion = homeworkResource.Descripcion,
            Fecha = homeworkResource.Fecha,
            Prioridad = (int)homeworkResource.PriorityTask,
            ImagenBase64 = homeworkResource.ImagenBase64 // Asignar la nueva propiedad
        };

        await _homeworkCommandService.UpdateHomeworkTask(command);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHomework(int id)
    {
        try
        {
            await _homeworkCommandService.DeleteHomeworkTask(id);
            return NoContent(); // Indica éxito sin contenido a devolver
        }
        catch (Exception ex)
        {
            // Manejo de excepciones (log, etc.)
            return StatusCode(500, "Error al eliminar la tarea");
        }
    }
}
