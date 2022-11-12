using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagementSystem.Data.Models
{
    public class ApplicationUserSubtask
    {
        [Required]
        public string ApplicationUserId { get; set; }
        
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public int SubtaskId { get; set; }

        [ForeignKey(nameof(SubtaskId))]
        public Subtask Subtask { get; set; }
    }
}
