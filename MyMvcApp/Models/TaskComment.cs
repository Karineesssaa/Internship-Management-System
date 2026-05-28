using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.Models
{
    public class TaskComment
    {
        public int Id { get; set; }

        [Required]
        public int TaskItemId { get; set; }

        public TaskItem? TaskItem { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        [Required]
        [StringLength(1000)]
        public string Text { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}