using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Data.Models
{
    public class Status
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(25)]
        public string StatusTitle { get; set; }

        public List<Subtask> Subtasks { get; set; } = new List<Subtask>();
    }
}
