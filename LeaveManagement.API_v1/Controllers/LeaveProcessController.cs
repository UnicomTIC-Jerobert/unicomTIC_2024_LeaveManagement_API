using LeaveManagement.API_v1.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.API_v1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveProcessController : ControllerBase
    {
        private readonly ILeaveProcessRepository _leaveProcessRepository;

        public LeaveProcessController(ILeaveProcessRepository leaveProcessRepository)
        {
            _leaveProcessRepository = leaveProcessRepository;
        }

        [HttpGet]
        public IActionResult GetAllLeaveProcesses()
        {
            var leaveProcesses = _leaveProcessRepository.GetAllLeaveProcesses();
            return Ok(leaveProcesses);
        }

        [HttpPost]
        public IActionResult AddLeaveProcessEntry([FromQuery] int employeeId, [FromQuery] int leaveRequestId, [FromBody] string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                return BadRequest("Status is required.");
            }

            _leaveProcessRepository.AddLeaveProcessEntry(employeeId, leaveRequestId, status);
            return Ok("Leave process updated successfully with new status.");
        }
    }

}
