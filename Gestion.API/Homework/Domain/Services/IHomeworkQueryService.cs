using Gestion.API.Homework.Domain.Model.Aggregates;
using Gestion.API.Homework.Domain.Model.Queries;

namespace Gestion.API.Homework.Domain.Services;

public interface IHomeworkQueryService
{
    Task<IEnumerable<homework>> GetAllHomeworkTasks(GetAllHomeworkTasksQuery query);
}