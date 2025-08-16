using MediatR;
using TaskManagement.Domain;

namespace TaskManagement.Application.Queries;

public record GetTaskByIdQuery(int Id) : IRequest<TaskItem?>;