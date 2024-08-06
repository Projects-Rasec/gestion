using Gestion.API.Homework.Domain.Model.Commands;
using System.Threading.Tasks;

namespace Gestion.API.Homework.Domain.Services;

public interface IHomeworkCommandService
{
    Task CreateHomeworkTask(CreateHomeworkCommand command);
    Task UpdateHomeworkTask(UpdateHomeworkCommand command);
    Task DeleteHomeworkTask(int id);
}