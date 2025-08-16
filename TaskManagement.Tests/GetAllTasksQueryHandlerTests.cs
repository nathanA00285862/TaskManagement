using Moq;
using Xunit;
using TaskManagement.Application.Queries;
using TaskManagement.Application.Handlers;
using TaskManagement.Domain;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TaskManagement.Tests;

public class GetAllTasksQueryHandlerTests
{
    private readonly Mock<ITaskRepository> _mockRepo = new();
    private readonly GetAllTasksQueryHandler _handler;

    public GetAllTasksQueryHandlerTests()
    {
        _handler = new GetAllTasksQueryHandler(_mockRepo.Object);
    }

    [Fact]
    public async Task Handle_ReturnsAllTasks()
    {
        // Arrange
        var tasks = new List<TaskItem>
        {
            new TaskItem { Id = 1, Description = "Task 1" },
            new TaskItem { Id = 2, Description = "Task 2" }
        };
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(tasks);

        // Act
        var result = await _handler.Handle(new GetAllTasksQuery(), CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, t => t.Id == 1 && t.Description == "Task 1");
        Assert.Contains(result, t => t.Id == 2 && t.Description == "Task 2");
        _mockRepo.Verify(r => r.GetAllAsync(), Times.Once());
    }

    [Fact]
    public async Task Handle_EmptyList_ReturnsEmpty()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<TaskItem>());

        // Act
        var result = await _handler.Handle(new GetAllTasksQuery(), CancellationToken.None);

        // Assert
        Assert.Empty(result);
        _mockRepo.Verify(r => r.GetAllAsync(), Times.Once());
    }
}