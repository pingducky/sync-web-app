using Microsoft.AspNetCore.Components;
using TP_Messagerie.Data;
using TP_Messagerie.Services;

namespace TP_Messagerie.Components.Pages.Auth
{
    public partial class Login
    {

        #region Paramètres

        public class Credential { public string Username { get; set; } = ""; public string Password { get; set; } = ""; }
        private Credential Credentials { get; } = new Credential();
        public string? ErrorMessage { get; set; }
        public bool Loading { get; set; }

        #endregion

        #region Méthodes

        private async void OnValidSubmit()
        {
            Loading = true;
            var user = await AuthService.LoginAsync(Credentials.Username, Credentials.Password);
            if (user != null)
            {
                UserSession.UserId = user.Id;
                UserSession.UserName = user.Username;
                UserSession.Email = user.Email;
                UserSession.IsAuthenticated = true;
                NavigationManager.NavigateTo("/");
            }
            else
            {
                ErrorMessage = "Nom d'utilisateur ou mot de passe incorrect";
                Loading = false;
                StateHasChanged();
            }
        }

        #endregion

    }
}
