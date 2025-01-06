﻿using Microsoft.AspNetCore.Components;
using TP_Messagerie.Data;
using TP_Messagerie.Services;
using Cassandra;


namespace TP_Messagerie.Components.Pages.Auth
{
    public partial class Login
    {
        [Inject]
        private Cassandra.ISession CassandraSession { get; set; } = default!;

        #region Paramètres

        public class Credential { public string Username { get; set; } = ""; public string Password { get; set; } = ""; }
        private Credential Credentials { get; } = new Credential();
        public string? ErrorMessage { get; set; }
        public bool Loading { get; set; }

        private bool isHandlingConnectionChange = false;
        private bool isConnected { get; set; } = false;

        #endregion

        #region Méthodes

        private async void OnValidSubmit()
        {
            Loading = true;
            var user = await AuthService.LoginAsync(Credentials.Username, Credentials.Password);
            Log log = new Log(
                 username: Credentials.Username,
                 action: UserAction.Login,
                 timestamp: DateTime.Now,
                 details: user == null ? "Connexion fail" : "Connexion Successful"
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
