namespace TaskManager.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? User_Id { get; set; }
        public IList<TaskItem> Tasks { get; set; }
    }
}
