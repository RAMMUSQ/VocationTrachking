using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebApplication5.Data;
using WebApplication5.Models;

namespace WebApplication5.Infrastructure.Repositories
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public LeaveRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            _context.LeaveRequests.Add(leaveRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<LeaveRequest> GetLeaveRequestByIdAsync(int id)
        {
            return await _context.LeaveRequests.FindAsync(id);
        }

        public async Task<IEnumerable<LeaveRequest>> GetPendingLeaveRequestsAsync()
        {
            return await _context.LeaveRequests
                .Where(lr => !lr.Approved.HasValue)
                .ToListAsync();
        }

        public async Task UpdateLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            _context.LeaveRequests.Update(leaveRequest);
            await _context.SaveChangesAsync();
        }
        public async Task<AnnualLeaveRight> GetAnnualLeaveRightByUserIdAsync(int userId)
        {
            return await _context.AnnualLeaveRights.SingleOrDefaultAsync(alr => alr.UserId == userId);
        }

        public async Task UpdateAnnualLeaveRightAsync(AnnualLeaveRight annualLeaveRight)
        {
            _context.AnnualLeaveRights.Update(annualLeaveRight);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> HasUserTakenBirthdayLeaveThisYearAsync(int userId)
        {
            return await _context.LeaveRequests.AnyAsync(lr => lr.UserId == userId && lr.LeaveType == Leavetype.Birthday && lr.StartDate.Year == DateTime.UtcNow.Year);
        }

        public async Task<DateTime?> GetUserBirthdayAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return user?.Birthdate;
        }
        public async Task<bool> HasUserTakenBirthdayLeaveThisYear(int userId, int year)
        {
            return await _context.LeaveRequests
                .AnyAsync(lr => lr.UserId == userId && lr.LeaveType == Leavetype.Birthday && lr.StartDate.Year == year);
        }
    }
}