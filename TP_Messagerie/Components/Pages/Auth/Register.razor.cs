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
        private bool IsLoading { get; set; }

        #endregion


        #region Methods

        /// <summary>
        /// M�thode de validation du formulaire d'inscription.
        /// </summary>
        private async void OnValidSubmit()
        {
            var test = UserService.GetAllUsersAsync();

            IsLoading = true;
            User user = new User
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Username = credential.Username,
                Password = credential.Password
            };

            await AuthService.RegisterAsync(user);
            NavigationManager.NavigateTo("/");
        }

        #endregion
    }
}
