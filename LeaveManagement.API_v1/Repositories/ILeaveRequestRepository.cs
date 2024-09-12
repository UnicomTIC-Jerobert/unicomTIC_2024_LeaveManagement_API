using LeaveManagement.API_v1.Models;

namespace LeaveManagement.API_v1.Repositories
{
    public interface ILeaveRequestRepository
    {
        void AddLeaveRequest(LeaveRequest leaveRequest, int employeeId);

        Task<int> ApplyLeaveRequestAsync(LeaveRequest leaveRequest);
        List<LeaveRequest> GetAllLeaveRequests();
    }
}
