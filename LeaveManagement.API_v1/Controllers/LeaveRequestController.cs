using LeaveManagement.API_v1.Models;
using LeaveManagement.API_v1.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.API_v1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILeaveProcessRepository _leaveProcessRepository;

        public LeaveRequestController(ILeaveRequestRepository leaveRequestRepo, ILeaveProcessRepository leaveProcessRepo)
        {
            _leaveRequestRepository = leaveRequestRepo;
            _leaveProcessRepository = leaveProcessRepo;
        }


        [HttpPost]
        public IActionResult AddLeaveRequest([FromBody] LeaveRequest leaveRequest, [FromQuery] int employeeId)
        {
            if (leaveRequest == null)
            {
                return BadRequest("Leave request is null.");
            }

            _leaveRequestRepository.AddLeaveRequest(leaveRequest, employeeId);
            return Ok("Leave request created successfully.");
        }

        [HttpGet]
        public IActionResult GetAllLeaveRequests()
        {
            var leaveRequests = _leaveRequestRepository.GetAllLeaveRequests();
            return Ok(leaveRequests);
        }

        [HttpPost("apply")]
        public async Task<IActionResult> ApplyLeaveRequest([FromBody] LeaveRequest leaveRequest, [FromQuery] int employeeId)
        {
            if (leaveRequest == null)
                return BadRequest("Invalid leave request data.");

            var leaveRequestId = await _leaveRequestRepository.ApplyLeaveRequestAsync(leaveRequest);

            // After inserting LeaveRequest, create a new LeaveProcess entry with 'initiated' status
            await _leaveProcessRepository.InitiateLeaveProcessAsync(employeeId, leaveRequestId);

            return Ok(new { LeaveRequestId = leaveRequestId, Message = "Leave request initiated successfully." });
        }

      
    }

}
