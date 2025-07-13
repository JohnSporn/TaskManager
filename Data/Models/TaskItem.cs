namespace TaskManager.Data.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public bool IsComplete { get; set; }
        public Category Category { get; set; } = new();
        public string UserId { get; set; } = string.Empty;
        
        // New reminder features
        public DateTime? ReminderDate { get; set; }
        public bool ReminderEnabled { get; set; }
        public bool EmailNotificationSent { get; set; }
        
        // Collaborative features
        public string? AssignedToUserId { get; set; }
        public string? AssignedToUserName { get; set; }
        public List<string> SharedWithUserIds { get; set; } = new();
        public string? Notes { get; set; }
        
        // Additional tracking
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? CompletedDate { get; set; }
    }
    
    public enum Priority
    {
        Low,
        Medium, 
        High
    };
}
