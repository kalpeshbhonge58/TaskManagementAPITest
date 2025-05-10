//using Microsoft.EntityFrameworkCore;
//using TaskManagementAPI.Data;
//using TaskManagementAPI.Models;

//namespace TaskManagementAPI
//{
//    public static class SeedData
//    {
//        public static void Initialize(IServiceProvider serviceProvider)
//        {
//            using var context = new AppDbContext(
//                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

//            if (context.Users.Any())
//                return; // DB has been seeded

//            var users = new List<User>
//        {
//            new User { Username = "admin", Password = "admin123", Role = "Admin", Email = "admin@example.com" },
//            new User { Username = "user1", Password = "user123", Role = "User", Email = "user1@example.com" },
//            new User { Username = "user2", Password = "user123", Role = "User", Email = "user2@example.com" }
//        };

//            context.Users.AddRange(users);
//            context.SaveChanges();

//            var tasks = new List<Tasks>
//        {
//            new Tasks { Title = "Complete API", Description = "Finish the task management API", Status = "InProgress", DueDate = DateTime.UtcNow.AddDays(7), UserId = 2 },
//            new Tasks { Title = "Write Documentation", Description = "Document all endpoints", Status = "New", DueDate = DateTime.UtcNow.AddDays(10), UserId = 2 },
//            new Tasks { Title = "Review Code", Description = "Review team members' code", Status = "New", DueDate = DateTime.UtcNow.AddDays(5), UserId = 3 }
//        };

//            context.TASKs.AddRange(tasks);
//            context.SaveChanges();

//            var comments = new List<TaskComment>
//        {
//            new TaskComment { CommentText = "Make sure to include authentication", TaskId = 1, UserId = 1 },
//            new TaskComment { CommentText = "I'll start working on this tomorrow", TaskId = 1, UserId = 2 }
//        };

//            context.TaskComments.AddRange(comments);
//            context.SaveChanges();
//        }
//    }
//}
