using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        public TaskStatus Status { get; set; } = TaskStatus.New;

        [DataType(DataType.Date)]
        public DateTime? Deadline { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? AssignedToUserId { get; set; }
        public ApplicationUser? AssignedToUser { get; set; }

        public string? CreatedByUserId { get; set; }
        public ApplicationUser? CreatedByUser { get; set; }

        public int? InternshipGroupId { get; set; }
        public InternshipGroup? InternshipGroup { get; set; }

        public List<TaskComment> Comments { get; set; } = new();

        public List<Submission> Submissions { get; set; } = new();

        public Grade? Grade { get; set; }
    }
}