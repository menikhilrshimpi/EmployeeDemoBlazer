using EmployeeManagementSystem.Data.Model;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace EmployeeManagementSystem.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IAuthService _authService;
        private User _currentUser;

        public CustomAuthStateProvider(IAuthService authService)
        {
            _authService = authService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();

            _currentUser = await _authService.GetCurrentUserAsync();

            if (_currentUser != null)
            {
                var claims = new[] {
                    new Claim(ClaimTypes.Name, _currentUser.Username),
                    new Claim(ClaimTypes.Role, _currentUser.Role)
                };
                identity = new ClaimsIdentity(claims, "Custom authentication");
            }

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public async Task MarkUserAsAuthenticated(User user)
        {
            await _authService.SetCurrentUserAsync(user);
            _currentUser = user;

            var claims = new[] {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var identity = new ClaimsIdentity(claims, "Custom authentication");

            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _authService.SetCurrentUserAsync(null);
            _currentUser = null;

            var identity = new ClaimsIdentity();

            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
        }
    }
}
