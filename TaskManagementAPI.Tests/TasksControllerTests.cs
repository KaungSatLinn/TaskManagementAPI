using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementAPI.Controllers;
using TaskManagementAPI.Dtos;
using TaskManagementAPI.Models;
using TaskManagementAPI.Services;
using Xunit;

namespace TaskManagementAPI.Tests
{
    public class TasksControllerTests
    {
        private readonly Mock<ITaskService> _mockTaskService;
        private readonly TasksController _controller;

        public TasksControllerTests()
        {
            _mockTaskService = new Mock<ITaskService>();
            _controller = new TasksController(_mockTaskService.Object, Mock.Of<ILogger<TasksController>>());
        }

        [Fact]
        public async Task GetTasks_ReturnsOkResultWithListOfTasks()
        {
            // Arrange
            var expectedTasks = new List<TaskDto>
            {
                new TaskDto { TaskId = 1, Title = "Task 1" },
                new TaskDto { TaskId = 2, Title = "Task 2" }
            };
            _mockTaskService.Setup(service => service.GetAllTasksAsync()).ReturnsAsync(expectedTasks);

            // Act
            var result = await _controller.GetTasks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTasks = Assert.IsAssignableFrom<List<TaskDto>>(okResult.Value);
            Assert.Equal(expectedTasks.Count, returnedTasks.Count);
        }

        [Fact]
        public async Task GetTaskByIdAsync_ExistingId_ReturnsOkResult()
        {
            // Arrange
            var taskId = 1;
            var expectedTask = new UpdateTaskDto { TaskId = taskId, Title = "Test Task" };
            _mockTaskService.Setup(service => service.GetTaskByIdAsync(taskId)).ReturnsAsync(expectedTask);

            // Act
            var result = await _controller.GetTaskByIdAsync(taskId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTask = Assert.IsType<UpdateTaskDto>(okResult.Value);
            Assert.Equal(expectedTask, returnedTask);
        }

        [Fact]
        public async Task GetTaskByIdAsync_NonexistentId_ReturnsNotFound()
        {
            // Arrange
            var taskId = 999; // Non-existent ID
            _mockTaskService.Setup(service => service.GetTaskByIdAsync(taskId)).ThrowsAsync(new ArgumentException("Task Not Found."));

            // Act
            var result = await _controller.GetTaskByIdAsync(taskId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Task Not Found.", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateTaskAsync_ValidModel_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var newTaskDto = new CreateTaskDto { Title = "New Task", Description = "Description" };
            var createdTaskDto = new CreateTaskDto { TaskId = 1, Title = "New Task", Description = "Description" };
            _mockTaskService.Setup(service => service.CreateTaskAsync(newTaskDto)).ReturnsAsync(createdTaskDto);

            // Act
            var result = await _controller.CreateTaskAsync(newTaskDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetTaskByIdAsync", createdAtActionResult.ActionName);
            var returnedTask = Assert.IsType<CreateTaskDto>(createdAtActionResult.Value);
            Assert.Equal(createdTaskDto, returnedTask);
        }

        [Fact]
        public async Task CreateTaskAsync_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Title", "Title is required");

            // Act
            var result = await _controller.CreateTaskAsync(new CreateTaskDto());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateTaskAsync_ValidModel_ReturnsOkResult()
        {
            // Arrange
            var taskId = 1;
            var updateTaskDto = new UpdateTaskDto { TaskId = taskId, Title = "Updated Task" };
            var updatedTaskDto = new UpdateTaskDto { TaskId = taskId, Title = "Updated Task" };
            _mockTaskService.Setup(service => service.UpdateTaskAsync(taskId, updateTaskDto)).ReturnsAsync(updatedTaskDto);

            // Act
            var result = await _controller.UpdateTaskAsync(taskId, updateTaskDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTask = Assert.IsType<TaskDto>(okResult.Value);
            Assert.Equal(updatedTaskDto.TaskId, returnedTask.TaskId);
            Assert.Equal(updatedTaskDto.Title, returnedTask.Title);
        }

        [Fact]
        public async Task UpdateTaskAsync_NonexistentId_ReturnsNotFound()
        {
            // Arrange
            var taskId = 999; // Non-existent ID
            var updateTaskDto = new UpdateTaskDto { TaskId = taskId, Title = "Updated Task" };
            _mockTaskService.Setup(service => service.UpdateTaskAsync(taskId, updateTaskDto)).ThrowsAsync(new ArgumentException("Task Not Found."));

            // Act
            var result = await _controller.UpdateTaskAsync(taskId, updateTaskDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Task Not Found.", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateTaskAsync_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var taskId = 1;
            _controller.ModelState.AddModelError("Title", "Title is required");

            // Act
            var result = await _controller.UpdateTaskAsync(taskId, new UpdateTaskDto());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task DeleteTaskAsync_ExistingId_ReturnsOkResult()
        {
            // Arrange
            var taskId = 1;
            _mockTaskService.Setup(service => service.DeleteTaskAsync(taskId)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteTaskAsync(taskId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public async Task DeleteTaskAsync_NonexistentId_ReturnsNotFound()
        {
            // Arrange
            var taskId = 999; // Non-existent ID
            _mockTaskService.Setup(service => service.DeleteTaskAsync(taskId)).ThrowsAsync(new ArgumentException("Task Not Found."));

            // Act
            var result = await _controller.DeleteTaskAsync(taskId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Task Not Found.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetTaskPriorities_ReturnsOkResultWithListOfPriorities()
        {
            // Arrange
            var expectedPriorities = new List<TaskPriorities>
            {
                new TaskPriorities { PriorityId = 1, PriorityName = "High" },
                new TaskPriorities { PriorityId = 2, PriorityName = "Medium" },
                new TaskPriorities { PriorityId = 3, PriorityName = "Low" }
            };
            _mockTaskService.Setup(service => service.GetTaskPriorities()).ReturnsAsync(expectedPriorities);

            // Act
            var result = await _controller.GetTaskPriorities();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedPriorities = Assert.IsAssignableFrom<List<TaskPriorities>>(okResult.Value);
            Assert.Equal(expectedPriorities, returnedPriorities);
        }

        [Fact]
        public async Task GetTaskStatuses_ReturnsOkResultWithListOfStatuses()
        {
            // Arrange
            var expectedStatuses = new List<TaskStatuses>
            {
                new TaskStatuses { StatusId = 1, StatusName = "Not Started" },
                new TaskStatuses { StatusId = 2, StatusName = "In Progress" },
                new TaskStatuses { StatusId = 3, StatusName = "Completed" }
            };
            _mockTaskService.Setup(service => service.GetTaskStatuses()).ReturnsAsync(expectedStatuses);

            // Act
            var result = await _controller.GetTaskStatuses();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedStatuses = Assert.IsAssignableFrom<List<TaskStatuses>>(okResult.Value);
            Assert.Equal(expectedStatuses, returnedStatuses);
        }

    }
}

