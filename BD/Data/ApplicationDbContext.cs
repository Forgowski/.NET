using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BD.Models;

namespace BD.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<BD.Models.Course> Course { get; set; } = default!;
        public DbSet<BD.Models.Article> Article { get; set; } = default!;
        public DbSet<BD.Models.BuyCourses> BuyCourses { get; set; } = default!;
    }
}
