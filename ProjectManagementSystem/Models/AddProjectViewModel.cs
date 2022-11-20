using ProjectManagementSystem.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Models
{
    public class AddProjectViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<ApplicationUser> ProjectManagers { get; set; } = new List<ApplicationUser>();

        public string ProjectManagerId { get; set; }

        public string Picture { get; set; }

    }
}
