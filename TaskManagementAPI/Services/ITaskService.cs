using TaskManagementAPI.Dtos;

namespace TaskManagementAPI.Services
{
    public interface ITaskService
    {
        Task<List<TaskDto>> GetAllTasksAsync();
        Task<TaskDto> GetTaskByIdAsync(int id);
        Task<TaskDto> CreateTaskAsync(CreateTaskDto taskDto);
        Task<TaskDto> UpdateTaskAsync(int id, UpdateTaskDto taskDto);
        Task DeleteTaskAsync(int id);
    }
}
