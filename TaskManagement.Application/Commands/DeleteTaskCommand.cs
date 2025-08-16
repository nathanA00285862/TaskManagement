using MediatR;

namespace TaskManagement.Application.Commands;

public record DeleteTaskCommand(int Id) : IRequest;