using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.Models
{
    public class InternshipGroup
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<TaskItem> Tasks { get; set; } = new();
    }
}