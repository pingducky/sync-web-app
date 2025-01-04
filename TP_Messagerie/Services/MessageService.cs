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

        public async Task SendMessagesAsync(List<MessageCollection> messages) =>
            await _messages.InsertManyAsync(messages);

        public async Task DeleteMessageAsync(string messageId) =>
            await _messages.DeleteOneAsync(m => m.Id == messageId);

        public async Task<List<MessageCollection>> GetMessagesBetweenUsersAsync(string loginUsernameSession, string contactUsername)
        {
            return await _messages.Find(m =>
                (m.Sender == loginUsernameSession && m.Receiver == contactUsername) ||
                (m.Sender == contactUsername && m.Receiver == loginUsernameSession)
            ).ToListAsync();
        }

        public async Task<MessageCollection> GetMessageByIdAsync(string messageId) =>
            await _messages.Find(m => m.Id == messageId).FirstOrDefaultAsync();

    }
}
