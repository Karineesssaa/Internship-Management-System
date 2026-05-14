namespace MyMvcApp.Models
{
    public class Grade
    {
        public int Id { get; set; }

        public int TaskItemId { get; set; }

        public TaskItem? TaskItem { get; set; }

        public string? StudentId { get; set; }

        public int Value { get; set; }

        public string? MentorComment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}