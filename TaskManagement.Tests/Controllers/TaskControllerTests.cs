using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using TaskManagement.Controllers;
using TaskManagement.DTOs;
using TaskManagement.Services.Interfaces;

namespace TaskManagement.Tests.Controllers;

public class TaskControllerTests
{
    [Fact]
    public async Task GetTasks_Returns_OkResult_With_Tasks()
    {

        var mockTaskService = new Mock<ITaskService>();
        var mockMemoryCache = new Mock<IMemoryCache>();

        var tasks = new List<TaskDTO>
            {
                new TaskDTO { Id = 1, Name = "Task 1", Description = "Description 1" },
                new TaskDTO { Id = 2, Name = "Task 2", Description = "Description 2" }
            };
        mockTaskService.Setup(service => service.GetAllTasks()).ReturnsAsync(tasks);

        var controller = new TaskController(mockTaskService.Object, mockMemoryCache.Object);

        var result = await controller.GetTasks();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var taskList = Assert.IsAssignableFrom<IEnumerable<TaskDTO>>(okResult.Value);
        Assert.Equal(2, taskList.Count());

    }

    [Fact]
    private static async Task GetTaskById_Returns_OkResult_With_Task()
    {
        int taskId = 1;
        var mockTaskService = new Mock<ITaskService>();
        var mockMemoryCache = new Mock<IMemoryCache>();

        var task = new TaskDTO { Id = taskId, Name = "Task 1", Description = "Description 1" };
        mockTaskService.Setup(service => service.GetTaskById(taskId)).ReturnsAsync(task);

        var controller = new TaskController(mockTaskService.Object, mockMemoryCache.Object);

        var result = await controller.GetTaskById(taskId);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var taskResult = Assert.IsAssignableFrom<TaskDTO>(okResult.Value);
        Assert.Equal(taskId, taskResult.Id);
        Assert.Equal(task.Name, taskResult.Name);
        Assert.Equal(task.Description, taskResult.Description);
    }

}
