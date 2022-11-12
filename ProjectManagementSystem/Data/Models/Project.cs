using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagementSystem.Data.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(25)]
        public string Name { get; set; }

        [Required]
        [StringLength(5000)]
        public string Description { get; set; } = "Add the task description here...";


        public string ProjectManagerId { get; set; }

        [Required]
        [ForeignKey(nameof(ProjectManagerId))]
        public ApplicationUser ProjectManager { get; set; }

        public List<Subtask> Subtasks { get; set; } = new List<Subtask>();
    }
}
