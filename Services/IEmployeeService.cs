using EmployeeManagementSystem.Data.Model;

namespace EmployeeManagementSystem.Services
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task AddEmployeeAsync(Employee employee);
        Task UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(int id);
        Task<List<Employee>> SearchEmployeesAsync(string searchTerm);
    }
}
