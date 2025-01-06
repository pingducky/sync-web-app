using Cassandra;
using System;
using System.Collections.Generic;
using TP_Messagerie.Data;

namespace TP_Messagerie.Services
{
    public enum UserAction
    {
        Login,
        Logout,
        Register,
        SendMessage,
        ReceiveMessage,
        CreateConversation,
        DeleteConversation,
        UpdateProfile,
        Other
    }

    public class LoggerService
    {
        private readonly Cassandra.ISession _cassandraSession;

        public LoggerService(Cassandra.ISession cassandraSession)
        {
            _cassandraSession = cassandraSession;
        }

        /// <summary>
        /// Log une action utilisateur dans la base de données Cassandra.
        /// </summary>
        /// <param name="username">Le nom d'utilisateur.</param>
        /// <param name="action">L'action effectuée.</param>
        /// <param name="details">Détails supplémentaires sur l'action (optionnel).</param>
        public void LogAction(string username, UserAction action, string? details = null)
        {
            try
            {
                var query = "INSERT INTO user_actions (username, action, timestamp, details) VALUES (?, ?, ?, ?)";
                var preparedStatement = _cassandraSession.Prepare(query);

                _cassandraSession.Execute(preparedStatement.Bind(
                    username,
                    action.ToString(),
                    DateTime.UtcNow,
                    details
                ));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la journalisation : {ex.Message}");
            }
        }

        /// <summary>
        /// Log un objet Log dans la base de données Cassandra.
        /// </summary>
        /// <param name="log">L'objet Log à journaliser.</param>
        public void Log(Log log)
        {
            try
            {
                var query = "INSERT INTO user_actions (username, action, timestamp, details) VALUES (?, ?, ?, ?)";
                var preparedStatement = _cassandraSession.Prepare(query);

                _cassandraSession.Execute(preparedStatement.Bind(
                    log.Username,
                    log.Action.ToString(),
                    DateTime.UtcNow,
                    log.Details
                ));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la journalisation de l'objet Log : {ex.Message}");
            }
        }

        /// <summary>
        /// Log une liste d'objets Log dans la base de données Cassandra.
        /// </summary>
        /// <param name="logs">La liste d'objets Log à journaliser.</param>
        public void Logs(List<Log> logs)
        {
            foreach (var log in logs)
            {
                Log(log);
            }
        }
    }
}
