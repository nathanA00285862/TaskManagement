using MediatR;
using TaskManagement.Domain;

namespace TaskManagement.Application.Commands;

public record CreateTaskCommand(string Description, string? Priority) : IRequest<TaskItem>;