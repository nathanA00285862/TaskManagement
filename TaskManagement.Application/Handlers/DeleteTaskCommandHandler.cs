using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskManagement.Application.Commands;
using TaskManagement.Domain;

namespace TaskManagement.Application.Handlers;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand>
{
    private readonly ITaskRepository _repository;

    public DeleteTaskCommandHandler(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id);
    }
}