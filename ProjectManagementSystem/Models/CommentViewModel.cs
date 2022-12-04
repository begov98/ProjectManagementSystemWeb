namespace ProjectManagementSystem.Models
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public string AuthorId { get; set; }

        public string Author { get; set; }

        public string CommentPost { get; set; }

        public int SubtaskId { get; set; }

        public string Subtask { get; set; }
    }
}
