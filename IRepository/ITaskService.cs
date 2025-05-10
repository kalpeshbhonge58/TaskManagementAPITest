using TaskManagementAPI.Models;

namespace TaskManagementAPI.IRepository
{
    public interface ITaskService
    {
        Task<Tasks> GetTask(int id);
        Task<List<Tasks>> GetAllTasks();
        Task<List<Tasks>> GetTasksByUser(int userId);
        Task<Tasks> CreateTask(Tasks task);
        Task<TaskComment> AddComment(TaskComment comment);
    }
}
