using TaskManagementAPI.Dtos;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Services
{
    public interface ITaskService
    {
        Task<List<TaskDto>> GetAllTasksAsync();
        Task<List<TaskPriorities>> GetTaskPriorities();
        Task<List<TaskStatuses>> GetTaskStatuses();
        Task<UpdateTaskDto> GetTaskByIdAsync(int id);
        Task<CreateTaskDto> CreateTaskAsync(CreateTaskDto taskDto);
        Task<UpdateTaskDto> UpdateTaskAsync(int id, UpdateTaskDto taskDto);
        Task<Boolean> DeleteTaskAsync(int id);
    }
}
