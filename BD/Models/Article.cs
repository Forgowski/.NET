using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BD.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }


        [Column(TypeName = "TEXT")]
        public string? TextContent { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? Title { get; set; }

        [ForeignKey("CourseId")]
        public int CourseId { get; set; }
        public Course? Course { get; set; }

    }
}
