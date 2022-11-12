using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace ProjectManagementSystem.Data.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        public string AuthorId { get; set; }

        [Required]
        [ForeignKey(nameof(AuthorId))]
        public ApplicationUser Author { get; set; }

        [Required]
        [StringLength(500)]
        public string CommentPost { get; set; }

    }
}
