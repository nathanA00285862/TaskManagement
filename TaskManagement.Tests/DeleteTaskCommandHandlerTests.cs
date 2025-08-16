using Moq;
using Xunit;
using TaskManagement.Application.Commands;
using TaskManagement.Application.Handlers;
using TaskManagement.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManagement.Tests;

public class DeleteTaskCommandHandlerTests
{
    private readonly Mock<ITaskRepository> _mockRepo = new();
    private readonly DeleteTaskCommandHandler _handler;

    public DeleteTaskCommandHandlerTests()
    {
        _handler = new DeleteTaskCommandHandler(_mockRepo.Object);
    }

    [Fact]
    public async Task Handle_DeletesTaskWhenFound()
    {
        // Arrange
        _mockRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(new DeleteTaskCommand(1), CancellationToken.None);

        // Assert
        _mockRepo.Verify(r => r.DeleteAsync(1), Times.Once());
    }
}