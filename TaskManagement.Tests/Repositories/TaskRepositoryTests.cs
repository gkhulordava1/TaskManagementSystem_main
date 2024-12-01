using System;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManagement.Data;
using TaskManagement.Repositories.Implementations;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Tests.Repositories;


public class TaskRepositoryTests
{
    private DbContextOptions<AppDbContext> _options;

    public TaskRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using (var context = new AppDbContext(_options))
        {
            context.Tasks.AddRange(new List<Models.Task>
                {
                    new Models.Task { Id = 1, Name = "Task 1", Description = "Description 1" },
                    new Models.Task { Id = 2, Name = "Task 2", Description = "Description 2" }
                });
            context.SaveChanges();
        }
    }

    [Fact]
    public async Task GetAllTasks_Returns_All_Tasks()
    {
      
        using (var context = new AppDbContext(_options))
        {
            var repository = new TaskRepository(context);

            var tasks = await repository.GetAllTasks();

            Assert.NotNull(tasks);
            Assert.Equal(2, tasks.Count()); 
        }
    }
}

