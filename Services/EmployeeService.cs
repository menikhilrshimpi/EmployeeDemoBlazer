using EmployeeManagementSystem.Data.Model;
using Newtonsoft.Json;

namespace EmployeeManagementSystem.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly string _filePath = "EmployeeData/employees.json";

        public async Task<List<Employee>> GetEmployeesAsync()
        {
            if (!File.Exists(_filePath)) return new List<Employee>();

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonConvert.DeserializeObject<List<Employee>>(json) ?? new List<Employee>();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            var employees = await GetEmployeesAsync();
            return employees.FirstOrDefault(e => e.Id == id);
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            var employees = await GetEmployeesAsync();
            employee.Id = employees.Any() ? employees.Max(e => e.Id) + 1 : 1;
            employees.Add(employee);
            await SaveEmployeesAsync(employees);
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            var employees = await GetEmployeesAsync();
            var index = employees.FindIndex(e => e.Id == employee.Id);
            if (index != -1)
            {
                employees[index] = employee;
                await SaveEmployeesAsync(employees);
            }
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employees = await GetEmployeesAsync();
            employees.RemoveAll(e => e.Id == id);
            await SaveEmployeesAsync(employees);
        }

        public async Task<List<Employee>> SearchEmployeesAsync(string searchTerm)
        {
            var employees = await GetEmployeesAsync();
            return employees.Where(e =>
                e.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                e.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                e.ProjectAccountName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                e.Designation.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }

        private async Task SaveEmployeesAsync(List<Employee> employees)
        {
            var json = JsonConvert.SerializeObject(employees, Formatting.Indented);
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}
