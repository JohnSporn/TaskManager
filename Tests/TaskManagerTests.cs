using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Data.Models;
using TaskManager.Data.Repositories;
using Xunit;

namespace TaskManager.Tests
{
    public class TaskManagerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public TaskManagerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", 
                response.Content.Headers.ContentType?.ToString());
        }
    }

    public class InMemoryTaskRepositoryTests
    {
        [Fact]
        public async Task Tasks_Get_ReturnsTasksForUser()
        {
            // Arrange
            var repository = new InMemoryTaskRepository();
            var testUserId = "test-user-id";

            // Act
            var tasks = new List<TaskItem?>();
            await foreach (var task in repository.Tasks_Get(testUserId))
            {
                tasks.Add(task);
            }

            // Assert
            Assert.NotEmpty(tasks);
            Assert.All(tasks, task => Assert.Equal(testUserId, task?.UserId));
        }

        [Fact]
        public async Task Task_Upsert_CreatesNewTask()
        {
            // Arrange
            var repository = new InMemoryTaskRepository();
            var newTask = new TaskItem
            {
                Name = "Test Task",
                DueDate = DateTime.Now.AddDays(1),
                Priority = Priority.Medium,
                Category = new Category { Id = 1, Name = "Test" },
                UserId = "test-user-id",
                ReminderEnabled = true,
                ReminderDate = DateTime.Now.AddHours(12)
            };

            // Act
            var result = await repository.Task_Upsert(newTask);

            // Assert
            Assert.True(result > 0);
            Assert.True(newTask.Id > 0);
        }

        [Fact]
        public async Task TaskItem_HasReminderFeatures()
        {
            // Arrange & Act
            var task = new TaskItem
            {
                Name = "Test Task with Reminder",
                ReminderEnabled = true,
                ReminderDate = DateTime.Now.AddDays(1),
                AssignedToUserName = "John Doe",
                SharedWithUserIds = new List<string> { "user1", "user2" }
            };

            // Assert
            Assert.True(task.ReminderEnabled);
            Assert.NotNull(task.ReminderDate);
            Assert.Equal("John Doe", task.AssignedToUserName);
            Assert.Equal(2, task.SharedWithUserIds.Count);
        }
    }
}