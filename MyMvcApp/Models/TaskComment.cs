using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.Models
{
    public class TaskComment
    {
        public int Id { get; set; }

        public int TaskItemId { get; set; }

        public TaskItem? TaskItem { get; set; }

        public string? UserId { get; set; }

        [Required]
        public string Text { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}