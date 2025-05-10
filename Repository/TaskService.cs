using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using TaskManagementAPI.IRepository;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Repository
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _dbContext;

        public TaskService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get Task by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Tasks> GetTask(int id)
        {
            return await _dbContext.TASKs.FirstOrDefaultAsync(t => t.Id == id);
        }

        /// <summary>
        /// Get All Tasks
        /// </summary>
        /// <returns></returns>
        public async Task<List<Tasks>> GetAllTasks()
        {
            return await _dbContext.TASKs.Include(t => t.User).ToListAsync();
        }

        /// <summary>
        /// Get Task by UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Tasks>> GetTasksByUser(int userId)
        {
            return await _dbContext.TASKs
                .Where(t => t.Id == userId)
                .Include(t => t.User)
                .ToListAsync();
        }

        /// <summary>
        ///  Create New Task
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Tasks> CreateTask(Tasks task)
        {
            try
            {
                task.User = null; // prevent EF from trying to insert a new User
                _dbContext.TASKs.Add(task);
                await _dbContext.SaveChangesAsync();

                // Reload the task with the User included
                return await _dbContext.TASKs
                    .Include(t => t.User)
                    .FirstOrDefaultAsync(t => t.Id == task.Id);
            }
            catch (Exception ex)
            {
                // Log or inspect error here
                throw new Exception("An error occurred while creating the task", ex);
            }
        }

        /// <summary>
        ///  Create New Comment
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<TaskComment> AddComment(TaskComment comment)
        {
            try
            {
                comment.User = null;
                _dbContext.TaskComments.Add(comment);
                await _dbContext.SaveChangesAsync();

                return await _dbContext.TaskComments
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == comment.Id);
            }
            catch (Exception ex)
            {
                // Log or inspect error here
                throw new Exception("An error occurred while creating the task", ex);
            }   
        }
    }
}
