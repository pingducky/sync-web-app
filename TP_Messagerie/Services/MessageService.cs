using MongoDB.Driver;
using TP_Messagerie.Data;

namespace TP_Messagerie.Services
{
    public class MessageService
    {
        private readonly IMongoCollection<Message> _messages;

        public MessageService(IMongoDatabase mongoDatabase)
        {
            _messages = mongoDatabase.GetCollection<Message>("messages");
        }

        public async Task SendMessageAsync(Message message) =>
            await _messages.InsertOneAsync(message);

        public async Task DeleteMessageAsync(string messageId) =>
            await _messages.DeleteOneAsync(m => m.Id == messageId);

        public async Task<IEnumerable<Message>> GetMessagesBetweenUsersAsync(string senderId, string receiverId) =>
            await _messages.Find(m => m.SenderId == senderId && m.ReceiverId == receiverId).ToListAsync();

        public async Task<Message> GetMessageByIdAsync(string messageId) =>
            await _messages.Find(m => m.Id == messageId).FirstOrDefaultAsync();

    }
}
