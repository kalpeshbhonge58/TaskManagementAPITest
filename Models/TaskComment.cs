namespace TaskManagementAPI.Models
{
    public class TaskComment
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
