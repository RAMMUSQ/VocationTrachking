using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface ILeaveRequestRepository
    {
        Task AddLeaveRequestAsync(LeaveRequest leaveRequest);
        Task<LeaveRequest> GetLeaveRequestByIdAsync(int id);
        Task<IEnumerable<LeaveRequest>> GetPendingLeaveRequestsAsync();
        Task UpdateLeaveRequestAsync(LeaveRequest leaveRequest);
    }
}