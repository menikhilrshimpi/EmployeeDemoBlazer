using EmployeeManagementSystem.Data.Model;
using Newtonsoft.Json;

namespace EmployeeManagementSystem.Services
{
    public class AuthService : IAuthService
    {
        private User _currentUser;
        private readonly IWebHostEnvironment _env;
        private readonly string _filePath;
        public AuthService(IWebHostEnvironment env)
        {
            _env = env;
            _filePath = Path.Combine(_env.WebRootPath, "UsersData", "users.json"); // Path to the file
        }
        public async Task SetCurrentUserAsync(User user)
        {
            _currentUser = user;
            // You could persist this in session/local storage if needed
        }

        public async Task<User> GetCurrentUserAsync()
        {
            return _currentUser;
        }
        public async Task<User> LoginAsync(string username, string password)
        {
            var users = await GetUsersAsync();
            return users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public async Task RegisterAsync(User user)
        {
            var users = await GetUsersAsync();
            users.Add(user);
            await SaveUsersAsync(users);
        }

        public async Task<List<User>> GetUsersAsync()
        {
            if (!File.Exists(_filePath)) return new List<User>();

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
        }

        private async Task SaveUsersAsync(List<User> users)
        {
            var json = JsonConvert.SerializeObject(users, Formatting.Indented);
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}
