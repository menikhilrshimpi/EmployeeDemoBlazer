using EmployeeManagementSystem.Data.Model;

namespace EmployeeManagementSystem.Services
{
    public interface IAuthService
    {
        Task<User> LoginAsync(string username, string password);
        Task RegisterAsync(User user);
        Task<List<User>> GetUsersAsync();
        Task SetCurrentUserAsync(User user);
        Task<User> GetCurrentUserAsync();
    }
}
