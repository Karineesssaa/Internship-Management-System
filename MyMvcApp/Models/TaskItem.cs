using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace MyMvcApp.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        public TaskStatus Status { get; set; } = TaskStatus.New;

        public DateTime? Deadline { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? AssignedToUserId { get; set; }

        public string? CreatedByUserId { get; set; }

        public int? InternshipGroupId { get; set; }

        public InternshipGroup? InternshipGroup { get; set; }

        public List<TaskComment> Comments { get; set; } = new();

        public List<Submission> Submissions { get; set; } = new();

        public Grade? Grade { get; set; }
    }
}