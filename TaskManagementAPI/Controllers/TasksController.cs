﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskManagementAPI.Dtos;
using TaskManagementAPI.Helper;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TasksController> _logger;
        public TasksController(ITaskService taskService, ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<TaskDto>>> GetTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        [HttpGet("priorities")]
        public async Task<ActionResult<List<TaskDto>>> GetTaskPriorities()
        {
            var priorities = await _taskService.GetTaskPriorities();
            return Ok(priorities);
        }

        [HttpGet("statuses")]
        public async Task<ActionResult<List<TaskDto>>> GetTaskStatuses()
        {
            var statuses = await _taskService.GetTaskStatuses();
            return Ok(statuses);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<TaskDto>> GetTaskByIdAsync(int id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                return Ok(task);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CreateTaskDto>> CreateTaskAsync(CreateTaskDto taskDto)
        {
            if (!ModelState.IsValid) // Checking the model state to ensure data integrity
            {
                return BadRequest(ModelState);
            }
            try
            {
                var task = await _taskService.CreateTaskAsync(taskDto);
                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CreateTaskAsync));
                return StatusCode(500, Messages.GlobalError);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<TaskDto>> UpdateTaskAsync(int id, UpdateTaskDto taskDto)
        {
            if (!ModelState.IsValid) // Checking the model state to ensure data integrity
            {
                throw new ArgumentException(ModelState.ToString()); // or any other error message
            }
            try
            {
                var task = await _taskService.UpdateTaskAsync(id, taskDto);
                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("delete/{id}")]
        public async Task<ActionResult<Boolean>> DeleteTaskAsync(int id)
        {
            try
            {
                var task = await _taskService.DeleteTaskAsync(id);
                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
