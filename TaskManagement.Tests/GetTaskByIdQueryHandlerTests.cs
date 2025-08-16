using Moq;
using Xunit;
using TaskManagement.Application.Queries;
using TaskManagement.Application.Handlers;
using TaskManagement.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManagement.Tests;

public class GetTaskByIdQueryHandlerTests
{
    private readonly Mock<ITaskRepository> _mockRepo = new();
    private readonly GetTaskByIdQueryHandler _handler;

    public GetTaskByIdQueryHandlerTests()
    {
        _handler = new GetTaskByIdQueryHandler(_mockRepo.Object);
    }

    [Fact]
    public async Task Handle_ReturnsTaskWhenFound()
    {
        // Arrange
        var task = new TaskItem { Id = 1, Description = "Test Task" };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(task);

        // Act
        var result = await _handler.Handle(new GetTaskByIdQuery(1), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Task", result.Description);
        _mockRepo.Verify(r => r.GetByIdAsync(1), Times.Once());
    }

    [Fact]
    public async Task Handle_ReturnsNullWhenNotFound()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((TaskItem?)null);

        // Act
        var result = await _handler.Handle(new GetTaskByIdQuery(999), CancellationToken.None);

        // Assert
        Assert.Null(result);
        _mockRepo.Verify(r => r.GetByIdAsync(999), Times.Once());
    }
}