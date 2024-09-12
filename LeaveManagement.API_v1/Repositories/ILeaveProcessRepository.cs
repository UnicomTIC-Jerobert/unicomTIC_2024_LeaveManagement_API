using LeaveManagement.API_v1.Models;

namespace LeaveManagement.API_v1.Repositories
{
    public interface ILeaveProcessRepository
    {
        Task InitiateLeaveProcessAsync(int employeeId, int leaveRequestId);

        Task UpdateLeaveProcessAsync(int leaveRequestId, string newStatus);
        List<LeaveProcess> GetAllLeaveProcesses();
        void AddLeaveProcessEntry(int employeeId, int leaveRequestId, string status);
    }
}
