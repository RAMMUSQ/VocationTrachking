using Application.Interfaces;
using Domain.Core;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _context;

        public LeaveRequestService(ILeaveRequestRepository leaveRequestRepository, IUserRepository userRepository,
            ApplicationDbContext context)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _userRepository = userRepository;
            _context = context;
        }

        public async Task<bool> CreateLeaveRequestAsync(int userId, Leavetype leaveType, DateTime startDate,
            DateTime endDate, string reason, string username)
        {
            var leaveRequest = new LeaveRequest
            {
                Username = username,
                UserId = userId,
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
            
            if (leaveRequest.LeaveType == Leavetype.Birthday)
            {
                var user = await _userRepository.GetUserByIdAsync(leaveRequest.UserId);
                if (user == null)
                {
                    Console.WriteLine("User not found with Id: " + leaveRequest.UserId);
                    return false;
                }

                var currentYear = DateTime.UtcNow.Year;
                var userBirthdayThisYear = new DateTime(currentYear, user.Birthdate.Month, user.Birthdate.Day);
                var userBirthdayThisYearAddOneDay = userBirthdayThisYear.AddDays(1);
                if (DateTime.UtcNow < userBirthdayThisYear && DateTime.UtcNow > userBirthdayThisYearAddOneDay)
                {
                    Console.WriteLine("Birthday leave cannot be taken before the user's birthday.");
                    return false;
                }

                var hasTakenBirthdayLeaveThisYear =
                    await _leaveRequestRepository.HasUserTakenBirthdayLeaveThisYear(leaveRequest.UserId, currentYear);
                if (hasTakenBirthdayLeaveThisYear)
                {
                    Console.WriteLine("Birthday leave has already been taken this year.");
                    return false;
                }

                if ((leaveRequest.EndDate.Date - leaveRequest.StartDate.Date).TotalDays != 1)
                {
                    Console.WriteLine("Birthday leave can only be 1 day.");
                    return false;
                }
            }

            // Leave request oluştur
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
                Console.WriteLine("Leave request is already approved or rejected. Approved value: " +
                                  existingRequest.Approved);
                return false;
            }


            existingRequest.Approved = true;

            // Yıllık izin mi kontrol et
            var leaveRight = await _leaveRequestRepository.GetAnnualLeaveRightByUserIdAsync(existingRequest.UserId);
            if (leaveRight != null)
            {
                var totalLeaveDays = CalculateBusinessDays(existingRequest.StartDate, existingRequest.EndDate);

                if (existingRequest.LeaveType == Leavetype.Annual)
                {
                    // Yıllık izinden düş
                    leaveRight.LeaveDays -= totalLeaveDays;
                }
                else
                {
                    // Yıllık izin dışı izinden günleri artır
                    leaveRight.NonAnnualLeaveDays += totalLeaveDays;
                }

                await _leaveRequestRepository.UpdateAnnualLeaveRightAsync(leaveRight);
            }

            if (leaveRequest.LeaveType == Leavetype.Birthday)
            {
                
            }

            await _leaveRequestRepository.UpdateLeaveRequestAsync(existingRequest);
            return true;
        }

        private async Task<bool> HasUserTakenBirthdayLeaveThisYear(int userId, int year)
        {
            return await _context.LeaveRequests
                .AnyAsync(lr => lr.UserId == userId && lr.LeaveType == Leavetype.Birthday && lr.StartDate.Year == year);
        }


        // Hafta sonlarını çıkartarak iş günü sayısını hesaplayan yardımcı metot
        private int CalculateBusinessDays(DateTime startDate, DateTime endDate)
        {
            int totalDays = (endDate - startDate).Days + 1;
            int businessDays = 0;

            for (int i = 0; i < totalDays; i++)
            {
                DateTime currentDate = startDate.AddDays(i);
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    businessDays++;
                }
            }

            return businessDays;
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