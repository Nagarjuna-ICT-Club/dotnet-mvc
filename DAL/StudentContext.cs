using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

namespace MvcMovie.DAL
{
    public class SchoolContext : DbContext
    {
#nullable disable
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {

        }

        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }

        public DbSet<Student> Students { get; set; }
    }
}