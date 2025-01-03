using Microsoft.AspNetCore.Components;
using MongoDB.Bson;
using System.Runtime.CompilerServices;
using TP_Messagerie.Data;
using TP_Messagerie.Services;

namespace TP_Messagerie.Components.Pages.Auth
{
    public partial class Register
    {
        #region Paramètres

        public class Credential {
            public string Username { get; set; } = "";
            public string Password { get; set; } = "";
            public string Firstname { get; set; } = "";
            public string Lastname { get; set; } = "";
            public string Name { get; set; } = "";
        }
        private Credential credential = new Credential();
        public bool IsLoading { get; set; }
        public string? ErrorMessage { get; set; }

        #endregion

        #region Méthodes
        private async void OnValidSubmit()
        {
            var test = await UserService.GetAllUsersAsync();

            IsLoading = true;
            User user = new User
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Username = credential.Username,
                Password = credential.Password,
                Firstname = credential.Firstname,
                Name = credential.Name,
            };

            bool result = await AuthService.RegisterAsync(user);
            if(result)
            {
                UserSession.UserId = user.Id;
                UserSession.UserName = user.Username;
                UserSession.Email = user.Email;
                UserSession.IsAuthenticated = true;
                NavigationManager.NavigateTo("/");
            }
            else
            {
                ErrorMessage = "L'utilisateur existe déjà !";
                IsLoading = false;
            }

        }
        #endregion
    }
}
