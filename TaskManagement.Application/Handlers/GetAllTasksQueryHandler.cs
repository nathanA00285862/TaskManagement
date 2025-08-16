using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskManagement.Application.Queries;
using TaskManagement.Domain;

namespace TaskManagement.Application.Handlers;

public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, IEnumerable<TaskItem>>
{
    private readonly ITaskRepository _repository;

    public GetAllTasksQueryHandler(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TaskItem>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync();
    }
}