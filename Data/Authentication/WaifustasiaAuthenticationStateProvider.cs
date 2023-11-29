using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Waifustasia.Data.User;

namespace Waifustasia.Data.Authentication
{
    public class WaifustasiaAuthenticationStateProvider : AuthenticationStateProvider, IDisposable
    {
        private readonly IUserService userService;
        public event EventHandler<UserChangedEventArgs>? CurrentUserChanged;

        private User.User? _currentUser;
        public User.User? CurrentUser
        {
            get => _currentUser;
            private set
            {
                if (_currentUser != value)
                {
                    _currentUser = value;
                    OnCurrentUserChanged(new UserChangedEventArgs(value));
                }
            }
        }
        public bool IsAuthenticated()
        {
            if (CurrentUser is null)
            {
                return false;
            }

            try
            {
                var what = CurrentUser.ToClaimsPrincipal();
                return true;
            }
            catch (Exception)
            {
            }

            return false;
        }
        protected virtual void OnCurrentUserChanged(UserChangedEventArgs e)
        {
            CurrentUserChanged?.Invoke(this, e);
        }

        public WaifustasiaAuthenticationStateProvider(IUserService userService)
        {
            this.userService = userService;
            AuthenticationStateChanged += OnAuthenticationStateChangedAsync;
        }

        private async void OnAuthenticationStateChangedAsync(Task<AuthenticationState> task)
        {
            var authenticationState = await task;

            if (authenticationState is not null)
            {
                CurrentUser = User.User.FromClaimsPrincipal(authenticationState.User);
            }
            else
            {
                CurrentUser = null;
            }
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var principal = new ClaimsPrincipal();
            try
            {
                var user = await userService.FetchUserFromBrowserAsync();

                if (user is not null)
                {
                    var userInDatabase = await userService.GetUserByIdAsync(user.Id);

                    if (userInDatabase is not null)
                    {
                        principal = userInDatabase.ToClaimsPrincipal();
                        CurrentUser = userInDatabase;
                    }
                }

                return new(principal);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new(principal);
            }
        }

        public async Task LoginAsync(string email, string password)
        {
            var principal = new ClaimsPrincipal();
            var user = await userService.GetUserByEmailAndPasswordAsync(email, password);

            if (user is not null)
            {
                await userService.PersistUserToBrowserAsync(user);
                principal = user.ToClaimsPrincipal();
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }

        public async Task LogoutAsync()
        {
            await userService.ClearBrowserUserDataAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new())));
        }

        public void Dispose() => AuthenticationStateChanged -= OnAuthenticationStateChangedAsync;
    }
    public class UserChangedEventArgs : EventArgs
    {
        public User.User? NewUser { get; }

        public UserChangedEventArgs(User.User? newUser)
        {
            NewUser = newUser;
        }
    }
}