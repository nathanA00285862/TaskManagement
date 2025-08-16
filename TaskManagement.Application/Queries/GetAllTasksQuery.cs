using MediatR;
using TaskManagement.Domain;
using System.Collections.Generic;

namespace TaskManagement.Application.Queries;

public record GetAllTasksQuery : IRequest<IEnumerable<TaskItem>>;