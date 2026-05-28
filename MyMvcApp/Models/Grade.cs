using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.Models
{
    public class Grade
    {
        public int Id { get; set; }

        [Required]
        public int TaskItemId { get; set; }

        public TaskItem? TaskItem { get; set; }

        public string? StudentId { get; set; }
        public ApplicationUser? Student { get; set; }

        [Range(0, 100)]
        public int Value { get; set; }

        [StringLength(1000)]
        public string? MentorComment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}