using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteBlog.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }
        [Required]
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Content { get; set; }
        [Required]
        public DateTime CreateAt { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }

    }
}
