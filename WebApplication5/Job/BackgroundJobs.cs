/*using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Presentation.Models;

namespace WebApplication5.Job
{
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

                    var lastYearUpdate = leaveRight.LastUpdated.Year;
                    var currentYear = DateTime.UtcNow.Year;

                    // Geçen yılların sayısını hesaplayın ve her yıl için 14 gün ekleyin
                    for (int year = lastYearUpdate + 1; year <= currentYear; year++)
                    {
                        leaveRight.LeaveDays += 14;
                    }

                    // Güncellenen yılı kaydedin
                    leaveRight.LastUpdated = DateTime.UtcNow;
                }
                else
                {
                    // Kullanıcının yıllık izin kaydı yoksa yeni bir kayıt oluştur
                    var totalDaysWorked = (DateTime.UtcNow - user.StartDateForworks).TotalDays;
                    int yearsWorked = (int)(totalDaysWorked / 365);

                    leaveRight = new AnnualLeaveRight
                    {
                        UserId = user.Id,
                        LeaveDays = yearsWorked * 14,
                        LastUpdated = DateTime.UtcNow
                    };
                    await _context.AnnualLeaveRights.AddAsync(leaveRight);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
*/