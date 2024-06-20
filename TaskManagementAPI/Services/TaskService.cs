using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using TaskManagementAPI.Dtos;

namespace TaskManagementAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly DataContext _dbContext;

        public TaskService(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<TaskDto> CreateTaskAsync(CreateTaskDto taskDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTaskAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TaskDto>> GetAllTasksAsync()
        {
            return await _dbContext.Tasks
                .Include(t => t.TaskStatuses)
                .Include(t => t.TaskPriorities)
                .Select(t => new TaskDto
                {
                    TaskId = t.TaskId,
                    Title = t.Title,
                    Description = t.Description,
                    DueDate = t.DueDate,
                    PriorityName = t.TaskPriorities.PriorityName,
                    StatusName = t.TaskStatuses.StatusName
                })
                .ToListAsync();
        }

        public Task<TaskDto> GetTaskByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<TaskDto> UpdateTaskAsync(int id, UpdateTaskDto taskDto)
        {
            throw new NotImplementedException();
        }
    }
}
