using Waifustasia.Pages.Account;

namespace Waifustasia.Data.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> LoginAsync(Login.LoginModel loginModel);
        bool IsUserAuthenticated();
    }
}