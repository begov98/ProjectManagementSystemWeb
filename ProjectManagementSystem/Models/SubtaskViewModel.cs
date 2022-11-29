using ProjectManagementSystem.Data.Models;

namespace ProjectManagementSystem.Models
{
    public class SubtaskViewModel
    {
            public int Id { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public int StatusId { get; set; }

            public string Status { get; set; }

            public int ProjectId { get; set; }

            public string Project { get; set; }

            public IEnumerable<Status> Statuses { get; set; } = new List<Status>();
            
    }
}
