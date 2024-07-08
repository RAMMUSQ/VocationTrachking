using System.Threading.Tasks;
using Core.Entities;

namespace Core.Services
{
    public interface ILeaveRequestService
    {
        Task<bool> CreateLeaveRequestAsync(LeaveRequest leaveRequest);
        Task<IEnumerable<LeaveRequest>> GetPendingLeaveRequestsAsync();
        Task<bool> ApproveLeaveRequestAsync(LeaveRequest leaveRequest);
        Task<bool> RejectLeaveRequestAsync(LeaveRequest leaveRequest);
        
    }
}