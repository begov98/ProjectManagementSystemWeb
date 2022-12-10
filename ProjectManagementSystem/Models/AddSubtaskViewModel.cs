using ProjectManagementSystem.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Models
{
    public class AddSubtaskViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Name { get; set; }
        [Required]
        [StringLength(5000, MinimumLength = 5)]
        public string Description { get; set; }

        [Required]
        public int StatusId { get; set; }

        public IEnumerable<Status> Statuses { get; set; } = new List<Status>();

        [Required]
        public int ProjectId { get; set; }

        public IEnumerable<Project> Projects { get; set; } = new List<Project>();

        [Required]
        public int CategoryId { get; set; }

        public IEnumerable<Category> Categories { get; set; } = new List<Category>();

    }
}
