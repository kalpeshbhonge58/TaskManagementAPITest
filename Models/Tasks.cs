namespace TaskManagementAPI.Models
{
    public class Tasks
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } // "New", "InProgress", "Completed"
        public DateTime DueDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
