using TP_Messagerie.Services;

namespace TP_Messagerie.Data
{
    public class Log
    {
        public string Username { get; set; }
        public UserAction Action { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Details { get; set; }

        public Log(string username, UserAction action, DateTime timestamp, string? details = null)
        {
            Username = username;
            Action = action;
            Timestamp = timestamp;
            Details = details;
        }
    }
}
