namespace LeaveManagement.API_v1.Models
{
    public class User
    {
        public int EmpId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // Admin, Manager, Employee
    }
}
