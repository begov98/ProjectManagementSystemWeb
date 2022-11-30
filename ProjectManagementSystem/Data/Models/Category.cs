using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Data.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<Subtask> Subtasks { get; set; } = new List<Subtask>();


    }
}
