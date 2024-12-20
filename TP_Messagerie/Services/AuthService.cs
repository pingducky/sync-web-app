using MongoDB.Driver;
using TP_Messagerie.Data;

namespace TP_Messagerie.Services
{
    public class AuthService
    {
        private readonly UserService _userService;
        public User? CurrentUser { get; set; }
        public bool IsAuthenticated => CurrentUser != null;

        public AuthService(UserService userService)
        {
            _userService = userService;
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            var user = await _userService.LoginAsync(username, password);
            if (user != null)
            {
                CurrentUser = user; 
            }
            return user;
        }

        public async Task<bool> RegisterAsync(User user)
        {
            var existingUser = await _userService.GetUserByUsernameAsync(user.Username);
            if (existingUser != null)
            {
                return false;
            }

            await _userService.CreateUserAsync(user);
            return true;
        }
    }
}
