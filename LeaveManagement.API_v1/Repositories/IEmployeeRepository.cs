using LeaveManagement.API_v1.Models;

namespace LeaveManagement.API_v1.Repositories
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAll();
        Employee GetById(int empId);
        void Add(Employee employee);
        void Update(Employee employee);
        void Delete(int empId);
    }
}
