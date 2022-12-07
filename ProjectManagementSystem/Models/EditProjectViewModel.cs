using ProjectManagementSystem.Data.Models;

namespace ProjectManagementSystem.Models
{
    public class EditProjectViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ProjectManagerId { get; set; }

        public string Picture { get; set; }

        public IEnumerable<ApplicationUser> ProjectManagers { get; set; } = new List<ApplicationUser>();
    }
}
