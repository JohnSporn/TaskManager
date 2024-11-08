namespace TaskManager.Data.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public bool IsComplete { get; set; }
    }
    public enum Priority
    {
        Low,
        Medium, 
        High
    };
}
