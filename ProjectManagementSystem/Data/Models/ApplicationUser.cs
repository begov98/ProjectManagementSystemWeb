using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Data.Models
{
    public class ApplicationUser :IdentityUser
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Position { get; set; } = "Employee";

        public byte[] ProfilePicture { get; set; } = new byte[16];

        public List<ApplicationUserSubtask> ApplicationUsersSubtasks { get; set; } = new List<ApplicationUserSubtask>();
    }
}
