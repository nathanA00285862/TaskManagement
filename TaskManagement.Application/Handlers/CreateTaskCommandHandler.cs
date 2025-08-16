using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskManagement.Application.Commands;
using TaskManagement.Domain;

namespace TaskManagement.Application.Handlers;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskItem>
{
    private readonly ITaskRepository _repository;

    public CreateTaskCommandHandler(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<TaskItem> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = new TaskItem
        {
            Description = request.Description,
            Priority = request.Priority
        };
        return await _repository.CreateAsync(task);
    }
}