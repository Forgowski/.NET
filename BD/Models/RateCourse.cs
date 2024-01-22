using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BD.Models
{
    public class RateCourse
    {
            
        [Key]
        public int RateId { get; set; }

        [ForeignKey("User")]
        [Required]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        [ForeignKey("Course")]
        [Required]
        public int CourseId { get; set; }
        public Course? Course { get; set; }
    }
}
