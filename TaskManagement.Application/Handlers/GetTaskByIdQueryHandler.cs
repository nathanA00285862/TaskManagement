using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskManagement.Application.Queries;
using TaskManagement.Domain;

namespace TaskManagement.Application.Handlers;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskItem?>
{
    private readonly ITaskRepository _repository;

    public GetTaskByIdQueryHandler(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<TaskItem?> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.Id);
    }
}