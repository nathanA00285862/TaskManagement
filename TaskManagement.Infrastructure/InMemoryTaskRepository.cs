using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Domain;

namespace TaskManagement.Infrastructure;

public class InMemoryTaskRepository : ITaskRepository
{
    private readonly List<TaskItem> _tasks = new();
    private int _nextId = 1;

    public async Task<TaskItem> CreateAsync(TaskItem task)
    {
        task.Id = _nextId++;
        _tasks.Add(task);
        return await Task.FromResult(task);
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await Task.FromResult(_tasks.FirstOrDefault(t => t.Id == id));
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        return await Task.FromResult(_tasks.AsEnumerable());
    }

    public async Task UpdateAsync(TaskItem task)
    {
        var existing = _tasks.FirstOrDefault(t => t.Id == task.Id);
        if (existing != null)
        {
            existing.Description = task.Description;
            existing.IsCompleted = task.IsCompleted;
            existing.Priority = task.Priority;
        }
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        _tasks.RemoveAll(t => t.Id == id);
        await Task.CompletedTask;
    }
}