using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BD.Models
{
    public class Quiz
    {
        [Key]
        public int QuizId { get; set; }

        [Required]
        [ForeignKey("Course")]
        public required int CourseId { get; set; }
        public Course? Course { get; set; }
    }
}
