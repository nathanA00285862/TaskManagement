using Moq;
using Xunit;
using TaskManagement.Application.Commands;
using TaskManagement.Application.Handlers;
using TaskManagement.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManagement.Tests;

public class UpdateTaskCommandHandlerTests
{
    private readonly Mock<ITaskRepository> _mockRepo = new();
    private readonly UpdateTaskCommandHandler _handler;

    public UpdateTaskCommandHandlerTests()
    {
        _handler = new UpdateTaskCommandHandler(_mockRepo.Object);
    }

    [Fact]
    public async Task Handle_UpdatesTaskWhenFound()
    {
        // Arrange
        var task = new TaskItem { Id = 1, Description = "Test Task", IsCompleted = false };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(task);
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<TaskItem>())).Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(new UpdateTaskCommand(1, true), CancellationToken.None);

        // Assert
        Assert.True(task.IsCompleted);
        _mockRepo.Verify(r => r.GetByIdAsync(1), Times.Once());
        _mockRepo.Verify(r => r.UpdateAsync(It.Is<TaskItem>(t => t.Id == 1 && t.IsCompleted)), Times.Once());
    }

    [Fact]
    public async Task Handle_DoesNothingWhenNotFound()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((TaskItem?)null);

        // Act
        await _handler.Handle(new UpdateTaskCommand(999, true), CancellationToken.None);

        // Assert
        _mockRepo.Verify(r => r.GetByIdAsync(999), Times.Once());
        _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<TaskItem>()), Times.Never());
    }
}