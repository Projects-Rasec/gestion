using Gestion.API.Homework.Domain.Model.Aggregates;
using Gestion.API.Homework.Domain.Repositories;
using Gestion.API.Shared.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gestion.API.Homework.Infrastructure.Persistence.JPA.Repositories;

public class HomeworkRepository : IHomeworkRepository
{
    private readonly AppDbContext _context;

    public HomeworkRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<homework>> GetAllAsync()
    {
        return await _context.HomeworkTasks.FromSqlRaw("SELECT Id, Nombre, Descripcion, Fecha, Prioridad AS PriorityTask, ImagenBase64 FROM HomeworkTasks").ToListAsync();
    }

    public async Task<IEnumerable<homework>> GetPagedAsync(int pageNumber, int pageSize)
    {
        var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
        var pageSizeParam = new SqlParameter("@PageSize", pageSize);
        
        return await _context.HomeworkTasks.FromSqlRaw("EXEC GetPagedHomeworkTasks @PageNumber, @PageSize", pageNumberParam, pageSizeParam).ToListAsync();
    }
    
    public async Task<int> GetTotalCountAsync()
    {
        return await _context.HomeworkTasks.CountAsync();
    }


    public async Task AddAsync(homework task)
    {
        var nombreParam = new SqlParameter("@Nombre", task.Nombre);
        var descripcionParam = new SqlParameter("@Descripcion", task.Descripcion);
        var fechaParam = new SqlParameter("@Fecha", task.Fecha);
        var prioridadParam = new SqlParameter("@Prioridad", (int)task.PriorityTask);
        var imagenBase64Param = new SqlParameter("@ImagenBase64", task.ImagenBase64);

        var idParam = new SqlParameter
        {
            ParameterName = "@Id",
            SqlDbType = System.Data.SqlDbType.Int,
            Direction = System.Data.ParameterDirection.Output
        };

        await _context.Database.ExecuteSqlRawAsync("EXEC AddHomework @Nombre, @Descripcion, @Fecha, @Prioridad, @ImagenBase64, @Id OUTPUT", nombreParam, descripcionParam, fechaParam, prioridadParam, imagenBase64Param, idParam);

        task.Id = (int)idParam.Value;
    }

    public async Task UpdateAsync(homework task)
    {
        var idParam = new SqlParameter("@Id", task.Id);
        var nombreParam = new SqlParameter("@Nombre", task.Nombre ?? (object)DBNull.Value); 
        var descripcionParam = new SqlParameter("@Descripcion", task.Descripcion ?? (object)DBNull.Value);
        var fechaParam = new SqlParameter("@Fecha", task.Fecha);
        var prioridadParam = new SqlParameter("@Prioridad", (int)task.PriorityTask);
        var imagenBase64Param = new SqlParameter("@ImagenBase64", task.ImagenBase64);

        await _context.Database.ExecuteSqlRawAsync("EXEC UpdateHomework @Id, @Nombre, @Descripcion, @Fecha, @Prioridad, @ImagenBase64", new[] { idParam, nombreParam, descripcionParam, fechaParam, prioridadParam, imagenBase64Param });
    }

    public async Task DeleteAsync(int id)
    {
        var idParam = new SqlParameter("@Id", id);

        await _context.Database.ExecuteSqlRawAsync("EXEC DeleteHomework @Id", idParam);
    }
}
