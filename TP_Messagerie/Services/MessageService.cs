using MongoDB.Driver;
using TP_Messagerie.Data;

namespace TP_Messagerie.Services
{
    public class MessageService
    {
        private readonly IMongoCollection<MessageCollection> _messages;

        public MessageService(IMongoDatabase mongoDatabase)
        {
            _messages = mongoDatabase.GetCollection<MessageCollection>("messages");
        }

        public async Task SendMessageAsync(MessageCollection message) =>
            await _messages.InsertOneAsync(message);

        public async Task DeleteMessageAsync(string messageId) =>
            await _messages.DeleteOneAsync(m => m.Id == messageId);

        //public async Task<IEnumerable<MessageCollection>> GetMessagesBetweenUsersAsync(string senderId, string receiverId) =>
        //    await _messages.Find(m => m.SenderId == senderId && m.ReceiverId == receiverId).ToListAsync();

        public async Task<MessageCollection> GetMessageByIdAsync(string messageId) =>
            await _messages.Find(m => m.Id == messageId).FirstOrDefaultAsync();

    }
}
