using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace Core.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;

        public LeaveRequestService(ILeaveRequestRepository leaveRequestRepository)
        {
            _leaveRequestRepository = leaveRequestRepository;
        }

        public async Task<bool> CreateLeaveRequestAsync(int userId, string leaveType, DateTime startDate, DateTime endDate, string reason,string username)
        {
            var leaveRequest = new LeaveRequest
            {
                Username = username,
                UserId  = userId ,
                LeaveType = leaveType,
                StartDate = startDate,
                EndDate = endDate,
                Reason = reason
            };

            await _leaveRequestRepository.AddLeaveRequestAsync(leaveRequest);
            return true;
        }

        public async Task<bool> CreateLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            

            await _leaveRequestRepository.AddLeaveRequestAsync(leaveRequest);
            return true;
        }
        

        public async Task<IEnumerable<LeaveRequest>> GetPendingLeaveRequestsAsync()
        {
            return await _leaveRequestRepository.GetPendingLeaveRequestsAsync();
        }

        public async Task<bool> ApproveLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            var existingRequest = await _leaveRequestRepository.GetLeaveRequestByIdAsync(leaveRequest.Id);
            if (existingRequest == null)
            {
                Console.WriteLine("Leave request not found with Id: " + leaveRequest.Id);
                return false;
            }
            if (existingRequest.Approved.HasValue)
            {
                Console.WriteLine("Leave request is already approved or rejected. Approved value: " + existingRequest.Approved);
                return false;
            }

            existingRequest.Approved = true;
            await _leaveRequestRepository.UpdateLeaveRequestAsync(existingRequest);
            return true;
        }

        public async Task<bool> RejectLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            var existingRequest = await _leaveRequestRepository.GetLeaveRequestByIdAsync(leaveRequest.Id);
            if (existingRequest == null)
            {
                Console.WriteLine($"Leave request with Id {leaveRequest.Id} not found.");
                return false;
            }

            if (existingRequest.Approved.HasValue)
            {
                Console.WriteLine($"Leave request with Id {leaveRequest.Id} is already approved or rejected.");
                return false;
            }

            existingRequest.Approved = false;
            await _leaveRequestRepository.UpdateLeaveRequestAsync(existingRequest);
            Console.WriteLine($"Leave request with Id {leaveRequest.Id} has been rejected.");
            return true;
        }
    }
}
