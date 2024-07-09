using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Presentation.Models;
using VocationScheduler;

public class AnnualLeaveService : IAnnualLeaveService
{
    private readonly ApplicationDbContext _context;

    public AnnualLeaveService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task UpdateAnnualLeave()
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

                // Son güncellenme tarihinden bu yana geçen yıl sayısını hesapla
                var lastUpdated = leaveRight.LastUpdated ?? user.StartDateForworks;
                int yearsSinceLastUpdate = (int)((DateTime.UtcNow - lastUpdated).TotalDays / 365);

                if (yearsSinceLastUpdate > 0)
                {
                    leaveRight.LeaveDays += yearsSinceLastUpdate * 14; // Her yıl için 14 gün ekle
                    leaveRight.LastUpdated = DateTime.UtcNow; // Son güncellenme tarihini ayarla
                }
            }
            else
            {
                // Eğer kullanıcıya ait bir yıllık izin hakkı yoksa, oluşturun
                var totalDaysWorked = (DateTime.UtcNow - user.StartDateForworks).TotalDays;
                int yearsWorked = (int)(totalDaysWorked / 365);

                leaveRight = new AnnualLeaveRight
                {
                    UserId = user.Id,
                    LeaveDays = yearsWorked * 14, // Her yıl için 14 gün
                    LastUpdated = DateTime.UtcNow // Son güncellenme tarihi
                };

                _context.AnnualLeaveRights.Add(leaveRight);
            }
        }

        await _context.SaveChangesAsync();
    }
}
