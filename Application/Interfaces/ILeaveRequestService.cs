using Domain.Core;

namespace Application.Interfaces
{
    public interface ILeaveRequestService
    {
        Task<bool> CreateLeaveRequestAsync(LeaveRequest leaveRequest);
        Task<IEnumerable<LeaveRequest>> GetPendingLeaveRequestsAsync();
        Task<bool> ApproveLeaveRequestAsync(LeaveRequest leaveRequest);
        Task<bool> RejectLeaveRequestAsync(LeaveRequest leaveRequest);
        
    }
}