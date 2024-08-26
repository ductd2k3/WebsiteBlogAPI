using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteBlog.Models
{
    public class Blog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BlogId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Content { get; set; }
        [Required]
        public string BlogImage { get; set; }
        [Required]
        public DateTime CreateAt { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public int UserId { get; set; }
        public User User { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
