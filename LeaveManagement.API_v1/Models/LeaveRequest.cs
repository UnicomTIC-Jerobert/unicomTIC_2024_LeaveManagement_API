namespace LeaveManagement.API_v1.Models
{
    public class LeaveRequest
    {
        public int Id { get; set; }
        public string TypeOfLeave { get; set; }
        public string Reason { get; set; }
        public DateTime ApplyingDate { get; set; }
        public DateTime LeaveDate { get; set; }
        public int NumberOfLeaveDays { get; set; }
    }

}
