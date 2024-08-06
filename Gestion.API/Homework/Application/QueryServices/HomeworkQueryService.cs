using Gestion.API.Homework.Domain.Model.Queries;
using Gestion.API.Homework.Domain.Repositories;
using Gestion.API.Homework.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gestion.API.Homework.Domain.Model.Aggregates;

namespace Gestion.API.Homework.Application.QueryServices;

public class HomeworkQueryService : IHomeworkQueryService
{
    private readonly IHomeworkRepository _homeworkRepository;

    public HomeworkQueryService(IHomeworkRepository homeworkRepository)
    {
        _homeworkRepository = homeworkRepository;
    }

    public async Task<IEnumerable<homework>> GetAllHomeworkTasks(GetAllHomeworkTasksQuery query)
    {
        return await _homeworkRepository.GetAllAsync();
    }
}