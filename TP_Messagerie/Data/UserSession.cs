namespace TP_Messagerie.Data
{
    public class UserSession
    {
        public string? UserId { get; set; } = null;
        public string? UserName { get; set; } = null;
        public string? Email { get; set; } = null;
        public string? Receiver { get; set; } = null;
        public bool IsAuthenticated { get; set; } = false;

        public void Clear()
        {
            UserId = null;
            UserName = null;
            Email = null;
            IsAuthenticated = false;
        }
    }

}
