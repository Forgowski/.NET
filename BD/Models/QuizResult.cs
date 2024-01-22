using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BD.Models
{
    public class QuizResult
    {
        [Key]
        public int QuizResultId { get; set; }

        [Required]
        [ForeignKey("Course")]
        public required int CourseId { get; set; }
        public Course? Course { get; set; }

        [ForeignKey("User")]
        [Required]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        [Column(TypeName = "int")]
        public int ResultPoint { get; set; }
    }
}
