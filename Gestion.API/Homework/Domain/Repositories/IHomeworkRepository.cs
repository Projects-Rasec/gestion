using Gestion.API.Homework.Domain.Model.Aggregates;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gestion.API.Homework.Domain.Repositories;

public interface IHomeworkRepository
{
    Task<IEnumerable<homework>> GetAllAsync();
    Task<IEnumerable<homework>> GetPagedAsync(int pageNumber, int pageSize);
    Task<int> GetTotalCountAsync();
    Task AddAsync(homework task);
    Task UpdateAsync(homework task);
    Task DeleteAsync(int id);
}