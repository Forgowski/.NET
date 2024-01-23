using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BD.Models
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }

        [Required]
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string QuestionText { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string AnswerA { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string AnswerB { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string AnswerC { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string AnswerD { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string AnswerCorrect { get; set; }

        [Column(TypeName = "int")]
        public int Point { get; set; }
    }
}
