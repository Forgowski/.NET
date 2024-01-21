using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BD.Models
{
    public class BuyCourses
    {
        [Key]
        public int BuyId { get; set; }

        [ForeignKey("User")]
        public required string UserId { get; set; }
        public IdentityUser User { get; set; }

        [ForeignKey("Course")]
        public required int CourseId { get; set; }
        public Course? Course { get; set; }
    }
}
