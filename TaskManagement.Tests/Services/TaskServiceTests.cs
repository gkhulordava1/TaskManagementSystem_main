using AutoMapper;
using Moq;
using TaskManagement.DTOs;
using TaskManagement.Repositories.Interfaces;
using TaskManagement.Services.Implementations;

namespace TaskManagement.Tests.Services;


public class TaskServiceTests
{
    [Fact]
    public async Task GetAllTasks_Returns_Expected_Tasks()
    {
       
        var mockTaskRepository = new Mock<ITaskRepository>();
        var mockMapper = new Mock<IMapper>();

        var tasks = new List<Models.Task>
            {
                new Models.Task { Id = 1, Name = "Task 1", Description = "Description 1" },
                new Models.Task { Id = 2, Name = "Task 2", Description = "Description 2" }
            };
        mockTaskRepository.Setup(repo => repo.GetAllTasks()).ReturnsAsync(tasks);

        var taskDTOs = new List<TaskDTO>
        {
            new TaskDTO { Id = 1, Name = "Task 1", Description = "Description 1" },
            new TaskDTO { Id = 2, Name = "Task 2", Description = "Description 2" }
        };

        mockMapper.Setup(mapper => mapper.Map<TaskDTO>(It.IsAny<Models.Task>()))
            .Returns((Models.Task source) =>
            {
                var taskDto = new TaskDTO();
                taskDto.Id = source.Id;
                taskDto.Name = source.Name;
                taskDto.Description = source.Description;
                return taskDto;
            });

        var service = new TaskService(mockTaskRepository.Object, mockMapper.Object);

       
        var result = await service.GetAllTasks();

       
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<TaskDTO>>(result);
        Assert.Equal(2, result.Count()); 
    }

}

