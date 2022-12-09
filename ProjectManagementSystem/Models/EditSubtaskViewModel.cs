using ProjectManagementSystem.Data.Models;

namespace ProjectManagementSystem.Models
{
    public class EditSubtaskViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    }
}
