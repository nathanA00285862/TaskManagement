using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagement.Domain;

public interface ITaskRepository
{
    Task<TaskItem> CreateAsync(TaskItem task);
    Task<TaskItem?> GetByIdAsync(int id);
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task UpdateAsync(TaskItem task);
    Task DeleteAsync(int id);
}