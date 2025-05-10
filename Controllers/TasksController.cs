using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagementAPI.IRepository;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Controllers
{
    // Controllers/TasksController.cs
    [ApiController]
    [Route("api/tasks")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }


        [HttpGet("all")]
        [Authorize]
        public async Task<IActionResult> GetAllTask()
        {
            var isAdmin = User.IsInRole("Admin");
            if (isAdmin == true)
            {
                var task = await _taskService.GetAllTasks();
                if (task == null || !task.Any())
                    return NotFound("No tasks found.");

                return Ok(task);
            }
            return Forbid();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var task = await _taskService.GetTask(id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpGet("user/{userId}")]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> GetTasksByUser(int userId)
        {
            // Get user ID from claims (more secure than User.Identity.Name)
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // If using JWT with 'sub' claim:
            // var currentUserId = User.FindFirstValue("sub");

            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && currentUserId != userId.ToString())
            {
                return Forbid();
            }

            var tasks = await _taskService.GetTasksByUser(userId);
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] Tasks task)
        {
            try
            {
                if (task == null)
                {
                    return BadRequest("Task object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdTask = await _taskService.CreateTask(task).ConfigureAwait(false);

                if (createdTask == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error creating task");
                }

                return CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, createdTask);
            }
            catch (Exception ex)
            {
                // Log the exception here (using your preferred logging framework)
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost("{taskId}/comments")]
        public async Task<IActionResult> AddComment(int taskId, [FromBody] TaskComment comment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            comment.TaskId = taskId;
            var createdComment = await _taskService.AddComment(comment);
            return Ok(createdComment);
        }
    }
}
