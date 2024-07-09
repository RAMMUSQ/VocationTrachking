using Microsoft.EntityFrameworkCore;

namespace VocationComman
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<AnnualLeaveRight> AnnualLeaveRights { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public DateTime StartDateForworks { get; set; }
        // Diğer kullanıcı özellikleri
    }

    public class AnnualLeaveRight
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LeaveDays { get; set; }
        public DateTime LastUpdated { get; set; }
        // Diğer yıllık izin hakkı özellikleri
    }
}