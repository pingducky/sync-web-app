using Microsoft.AspNetCore.Components;
using TP_Messagerie.Services;

namespace TP_Messagerie.Components.Pages.Auth
{
    public partial class Login
    {

        #region Parameters

        public class Credential { public string Username { get; set; } = ""; public string Password { get; set; } = ""; }
        private Credential Credentials { get; } = new Credential();
        public string? ErrorMessage { get; set; }
        public bool Loading { get; set; }

        #endregion


        #region Methods

        /// <summary>
        /// Méthode de validation du formulaire de connexion.
        /// </summary>
        private async void OnValidSubmit()
        {
            Loading = true;
            var user = await AuthService.LoginAsync(Credentials.Username, Credentials.Password);
            if (user != null)
            {
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
