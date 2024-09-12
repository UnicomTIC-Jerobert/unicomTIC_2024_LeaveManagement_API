namespace LeaveManagement.API_v1.Models
{
    public class LeaveProcess
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Status { get; set; }  // "initiated", "approved", "rejected"
        public int LeaveRequestId { get; set; }
        public DateTime DateCreated { get; set; }
    }

}
