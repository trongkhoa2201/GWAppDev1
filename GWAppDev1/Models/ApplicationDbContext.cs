using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace GWAppDev1.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<Trainer> Trainers   { get; set; }
        public DbSet<Trainee> Trainees { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<CourseCategory> courseCategories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseTrainer> CoursesTrainers { get; set; }
        public DbSet<CourseTrainee> CoursesTrainees { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}