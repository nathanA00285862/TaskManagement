using MediatR;

namespace TaskManagement.Application.Commands;

public record UpdateTaskCommand(int Id, bool IsCompleted) : IRequest;