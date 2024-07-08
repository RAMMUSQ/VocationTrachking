using Microsoft.EntityFrameworkCore;

namespace VocationScheduler;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}