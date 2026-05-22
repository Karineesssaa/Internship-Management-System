using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.Models
{
    public class InternshipGroup
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<TaskItem> Tasks { get; set; } = new();
    }
}