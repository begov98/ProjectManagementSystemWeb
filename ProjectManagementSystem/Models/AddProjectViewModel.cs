using ProjectManagementSystem.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Models
{
    public class AddProjectViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        [StringLength(5000, MinimumLength = 10)]
        public string Description { get; set; }

        public IEnumerable<ApplicationUser> ProjectManagers { get; set; } = new List<ApplicationUser>();

        [Required]
        public string ProjectManagerId { get; set; }

        [Required]
        public string Picture { get; set; }

    }
}
