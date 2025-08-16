using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskManagement.Application.Commands;
using TaskManagement.Domain;

namespace TaskManagement.Application.Handlers;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand>
{
    private readonly ITaskRepository _repository;

    public UpdateTaskCommandHandler(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _repository.GetByIdAsync(request.Id);
        if (task != null)
        {
            task.IsCompleted = request.IsCompleted;
            await _repository.UpdateAsync(task);
        }
    }
}