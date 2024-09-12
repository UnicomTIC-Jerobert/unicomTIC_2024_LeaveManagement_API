using LeaveManagement.API_v1.Models;
using LeaveManagement.API_v1.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.API_v1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{empId}")]
        public async Task<ActionResult<User>> GetUserById(int empId)
        {
            var user = await _userRepository.GetUserByIdAsync(empId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(User user)
        {
            await _userRepository.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { empId = user.EmpId }, user);
        }

        [HttpPut("{empId}")]
        public async Task<ActionResult> UpdateUser(int empId, User user)
        {
            if (empId != user.EmpId)
            {
                return BadRequest();
            }

            var existingUser = await _userRepository.GetUserByIdAsync(empId);
            if (existingUser == null)
            {
                return NotFound();
            }

            await _userRepository.UpdateUserAsync(user);
            return NoContent();
        }

        [HttpDelete("{empId}")]
        public async Task<ActionResult> DeleteUser(int empId)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(empId);
            if (existingUser == null)
            {
                return NotFound();
            }

            await _userRepository.DeleteUserAsync(empId);
            return NoContent();
        }
    }
}
