namespace TaskManagementAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // "Admin" or "User"
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        //public ICollection<Tasks> TASKs { get; set; }
        //public ICollection<TaskComment> Comments { get; set; }
    }
}
