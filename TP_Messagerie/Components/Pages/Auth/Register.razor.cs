using Microsoft.AspNetCore.Components;
using MongoDB.Bson;
using System.Runtime.CompilerServices;
using TP_Messagerie.Data;
using TP_Messagerie.Services;

namespace TP_Messagerie.Components.Pages.Auth
{
    public partial class Register
    {
        #region Parameters

        public class Credential { public string Username { get; set; } = ""; public string Password { get; set; } = ""; }
        private Credential credential = new Credential();
        public bool IsLoading { get; set; }
        public string? ErrorMessage { get; set; }

        #endregion


        #region Methods

        /// <summary>
        /// Méthode de validation du formulaire d'inscription.
        /// </summary>
        private async void OnValidSubmit()
        {
            var test = await UserService.GetAllUsersAsync();

            IsLoading = true;
            User user = new User
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Username = credential.Username,
                Password = credential.Password
            };

            bool result = await AuthService.RegisterAsync(user);
            if(result)
            {
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
