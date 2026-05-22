using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.Models
{
    public class Submission
    {
        public int Id { get; set; }

        [Required]
        public int TaskItemId { get; set; }

        public TaskItem? TaskItem { get; set; }

        public string? StudentId { get; set; }

        [Url]
        [StringLength(500)]
        public string? GitHubLink { get; set; }

        [StringLength(1000)]
        public string? Comment { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.Now;

        public string? FilePath { get; set; }
    }
}