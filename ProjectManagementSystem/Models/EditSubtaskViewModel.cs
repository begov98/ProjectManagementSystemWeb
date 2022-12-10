using ProjectManagementSystem.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Models
{
    public class EditSubtaskViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        [StringLength(5000, MinimumLength = 5)]
        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    }
}
