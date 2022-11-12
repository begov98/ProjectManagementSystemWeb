using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Data.Models
{
    public class Subtask
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(5000)]
        public string Description { get; set; } = "Add the task description here...";

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public int StatusId { get; set; }

        [Required]
        [ForeignKey(nameof(StatusId))]
        public Status Status { get; set; }

        public List<ApplicationUserSubtask> ApplicationUsersSubtasks { get; set; } = new List<ApplicationUserSubtask>();
    }
}
