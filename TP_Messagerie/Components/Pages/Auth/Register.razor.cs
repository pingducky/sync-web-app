using Microsoft.AspNetCore.Components;
using MongoDB.Bson;
using TP_Messagerie.Data;
using TP_Messagerie.Services;

namespace TP_Messagerie.Components.Pages.Auth
{
    public partial class Register
    {
        #region Paramètres
        [Inject]
        private Cassandra.ISession CassandraSession { get; set; } = default!;
        public class Credential
        {
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

        private bool shouldProcessUnsentLogs = false; // Indicateur pour différer le traitement des logs non envoyés
        #endregion

        #region Méthodes
        private async void OnValidSubmit()
        {
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
                details: result ? "Register Successful" : "Register fail"
            );

            if (isConnected)
            {
                var loggerService = new LoggerService(CassandraSession);
                loggerService.Log(log);
            }
            else
            {
                // Marque pour traitement ultérieur après rendu
                shouldProcessUnsentLogs = true;
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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (shouldProcessUnsentLogs)
            {
                shouldProcessUnsentLogs = false;

                // Récupérer et traiter les logs non envoyés
                List<Log>? savedUnsentLogs = await LocalStorage.GetItemAsync<List<Log>>("savedUnsentLogs") ?? new List<Log>();
                if (savedUnsentLogs.Count > 0)
                {
                    var loggerService = new LoggerService(CassandraSession);
                    loggerService.Logs(savedUnsentLogs);

                    // Supprime les logs après leur envoi
                    await LocalStorage.RemoveItemAsync("savedUnsentLogs");
                }
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
                    // Marque pour traitement ultérieur après rendu
                    shouldProcessUnsentLogs = true;
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
