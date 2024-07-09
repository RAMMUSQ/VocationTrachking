using Domain.Core;
using Presentation.Models;


namespace Application.Interfaces
{
    public interface ILeaveRequestRepository
    {
        Task AddLeaveRequestAsync(LeaveRequest leaveRequest);
        Task<LeaveRequest> GetLeaveRequestByIdAsync(int id);
        Task<IEnumerable<LeaveRequest>> GetPendingLeaveRequestsAsync();
        Task UpdateLeaveRequestAsync(LeaveRequest leaveRequest);
        Task<AnnualLeaveRight> GetAnnualLeaveRightByUserIdAsync(int userId);
        Task UpdateAnnualLeaveRightAsync(AnnualLeaveRight annualLeaveRight);
        Task<DateTime?> GetUserBirthdayAsync(int userId);
        
        Task<bool> HasUserTakenBirthdayLeaveThisYear(int userId, int year);
    }
}