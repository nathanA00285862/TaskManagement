using Moq;
using Xunit;
using TaskManagement.Application.Commands;
using TaskManagement.Application.Handlers;
using TaskManagement.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManagement.Tests;

public class CreateTaskCommandHandlerTests
{
    private readonly Mock<ITaskRepository> _mockRepo = new();
    private readonly CreateTaskCommandHandler _handler;

    public CreateTaskCommandHandlerTests()
    {
        _handler = new CreateTaskCommandHandler(_mockRepo.Object);
    }

    [Fact]
    public async Task Handle_CreatesTask()
    {
        var command = new CreateTaskCommand("Test", "High");
        var expected = new TaskItem { Description = "Test", Priority = "High" };
        _mockRepo.Setup(r => r.CreateAsync(It.IsAny<TaskItem>())).ReturnsAsync(expected);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal("Test", result.Description);
        _mockRepo.Verify(r => r.CreateAsync(It.IsAny<TaskItem>()), Times.Once());
    }
}