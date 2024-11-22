using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveRequestController(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateLeaveRequest([FromBody] LeaveRequestDto leaveRequestDto)
        {

            var leaveRequest = new LeaveRequest();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<LeaveRequestDto, LeaveRequest>());
            var mapper = config.CreateMapper();
            var destination = mapper.Map<LeaveRequest>(leaveRequestDto);
            
                var result = await _leaveRequestService.CreateLeaveRequestAsync(destination);

                if (result)
                    return Ok();
                return BadRequest();
            
            
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingLeaveRequests()
        {
            var requests = await _leaveRequestService.GetPendingLeaveRequestsAsync();
            return Ok(requests);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("approve")]
        public async Task<IActionResult> ApproveLeaveRequest([FromBody] LeaveRequest leaveRequest)
        {
            if (leaveRequest == null)
            {
                return BadRequest("Invalid leave request");
            }

            var result = await _leaveRequestService.ApproveLeaveRequestAsync(leaveRequest);
            if (result)
                return Ok();
    
            // Detaylı hata mesajı ekleyin
            return BadRequest("Failed to approve leave request. Please check if the request is already approved or if the request ID is correct.");
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("reject")]
        public async Task<IActionResult> RejectLeaveRequest([FromBody] LeaveRequest leaveRequest)
        {
            if (leaveRequest == null)
            {
                return BadRequest("Invalid leave request");
            }

            var result = await _leaveRequestService.RejectLeaveRequestAsync(leaveRequest);
            if (result)
            {
                return Ok();
            }

            return BadRequest("Failed to reject leave request");
        }

    }
}
