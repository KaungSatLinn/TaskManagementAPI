using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using TaskManagementAPI.Dtos;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly DataContext _dbContext;

        public TaskService(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TaskDto>> GetAllTasksAsync()
        {
            return await _dbContext.Tasks
                .Where(x=>x.IsDelete == 0)
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

        public async Task<List<TaskPriorities>> GetTaskPriorities()
        {
            return await _dbContext.TaskPriorities
                .ToListAsync();
        }

        public async Task<List<TaskStatuses>> GetTaskStatuses()
        {
            return await _dbContext.TaskStatuses
                .ToListAsync();
        }

        public async Task<UpdateTaskDto> GetTaskByIdAsync(int id)
        {
            var task = await _dbContext.Tasks.FirstOrDefaultAsync(x=>x.StatusId == id && x.IsDelete == 0);
            if(task == null)
                throw new ArgumentException("Task Not Found.");
            var taskdto = new UpdateTaskDto
            {
                TaskId = task.TaskId,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                PriorityId = task.PriorityId,
                StatusId = task.StatusId
            };
            return taskdto;
        }

        public async Task<CreateTaskDto> CreateTaskAsync(CreateTaskDto taskDto)
        {
            var task = new Tasks
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                PriorityId = taskDto.PriorityId,
                DueDate = taskDto.DueDate,
                StatusId = taskDto.StatusId
            };

            _dbContext.Tasks.Add(task);
            await _dbContext.SaveChangesAsync();
            return new CreateTaskDto
            {
                TaskId = task.TaskId,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                PriorityId = task.PriorityId,
                StatusId = task.StatusId
            };
        }

        public async Task<Boolean> DeleteTaskAsync(int id)
        {
            var task = await _dbContext.Tasks.FirstOrDefaultAsync(x => x.StatusId == id && x.IsDelete == 0);
            if (task == null)
                throw new ArgumentException("Task Not Found.");
            task.IsDelete = 1;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<UpdateTaskDto> UpdateTaskAsync(int id, UpdateTaskDto taskDto)
        {
            var task = await _dbContext.Tasks.FirstOrDefaultAsync(x => x.StatusId == id && x.IsDelete == 0);
            if (task == null)
                throw new ArgumentException("Task Not Found.");
            task.Title = taskDto.Title;
            task.Description = taskDto.Description;

            await _dbContext.SaveChangesAsync();
            return new UpdateTaskDto
            {
                TaskId = task.TaskId,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                PriorityId = task.PriorityId,
                StatusId = task.StatusId
            };
        }
    }
}
