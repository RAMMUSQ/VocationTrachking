using Microsoft.EntityFrameworkCore;
using WebApplication5.Data;
using WebApplication5.Models;

namespace WebApplication5.Job;

public class AnnualLeaveUpdateJob
{
    private readonly ApplicationDbContext _context;

    public AnnualLeaveUpdateJob(ApplicationDbContext context)
    {
        _context = context;
    }

   
    public async Task Execute()
    {
        var users = await _context.Users.ToListAsync();
        foreach (var user in users)
        {
            var leaveRight = await _context.AnnualLeaveRights
                .FirstOrDefaultAsync(lr => lr.UserId == user.Id);

            if (leaveRight != null)
            {
                var totalDaysWorked = (DateTime.UtcNow - user.StartDateForworks).TotalDays;
                int yearsWorked = (int)(totalDaysWorked / 365);
                leaveRight.LeaveDays = yearsWorked * 14; // Her yıl için 14 gün
                
            }
        }

        await _context.SaveChangesAsync();
    }
}