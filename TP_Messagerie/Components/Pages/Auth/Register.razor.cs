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
        [Inject]
        private Cassandra.ISession CassandraSession { get; set; } = default!;
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


        private bool isHandlingConnectionChange = false;
        private bool isConnected { get; set; } = false;

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

            Log log = new Log(
             username: user.Username,
             action: UserAction.Register,
             timestamp: DateTime.Now,
             details: result == null ? "Register fail" : "Register Successful"
            );

            if (isConnected)
            {
                var loggerService = new LoggerService(CassandraSession);
                loggerService.Log(log);
            }
            else
            {
                List<Log>? savedUnsentLogs = await LocalStorage.GetItemAsync<List<Log>>("savedUnsentLogs") ?? new List<Log>();
                log.Details = "No connexion";
                savedUnsentLogs.Add(log);
                await LocalStorage.SetItemAsync("savedUnsentLogs", savedUnsentLogs);
            }


            if (result)
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

        private async void HandleConnectionStatusChanged(bool isConnected)
        {
            if (isHandlingConnectionChange) return;
            isHandlingConnectionChange = true;
            try
            {
                this.isConnected = isConnected;

                if (isConnected)
                {
                    List<Log>? savedUnsentLogs = await LocalStorage.GetItemAsync<List<Log>>("savedUnsentLogs");

                    if (savedUnsentLogs != null && savedUnsentLogs.Count != 0)
                    {
                        // Envoi des logs non envoyés
                        var loggerService = new LoggerService(CassandraSession);
                        loggerService.Logs(savedUnsentLogs);

                        // Supprime les messages stockés après envoi
                        await LocalStorage.RemoveItemAsync("savedUnsentLogs");
                    }
                }
                else
                {
                }
            }
            finally
            {
                isHandlingConnectionChange = false;
            }
        }
        #endregion
    }
}
