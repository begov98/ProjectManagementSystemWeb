using ProjectManagementSystem.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Models
{
    public class AddSubtaskViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        public int StatusId { get; set; }

        public IEnumerable<Status> Statuses { get; set; } = new List<Status>();

        public int ProjectId { get; set; }

        public IEnumerable<Project> Projects { get; set; } = new List<Project>();

        public int CategoryId { get; set; }

        public IEnumerable<Category> Categories { get; set; } = new List<Category>();

    }
}
