namespace MyMvcApp.Models
{
    public class Submission
    {
        public int Id { get; set; }

        public int TaskItemId { get; set; }

        public TaskItem? TaskItem { get; set; }

        public string? StudentId { get; set; }

        public string? GitHubLink { get; set; }

        public string? Comment { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.Now;

        public string? FilePath { get; set; }
    }
}