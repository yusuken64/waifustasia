using Microsoft.AspNetCore.Identity;
using static Waifustasia.Pages.Account.Login;

namespace Waifustasia.Data.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly SignInManager<User.User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(SignInManager<User.User> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsUserAuthenticated()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        }

        public async Task<AuthenticationResult> LoginAsync(LoginModel loginModel)
        {
            var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return AuthenticationResult.Success;
            }
            else
            {
                return AuthenticationResult.Failure;
            }
        }
    }

    public enum AuthenticationResult
    {
        Success,
        Failure
    }
}
