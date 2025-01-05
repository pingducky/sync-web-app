using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using TP_Messagerie.Data;
using TP_Messagerie.Hubs;

namespace TP_Messagerie.Services
{
    public class MessageService
    {
        private readonly IMongoCollection<MessageCollection> _messages;
        private readonly IHubContext<MessageHub> _hubContext;

        public MessageService(IMongoDatabase mongoDatabase, IHubContext<MessageHub> hubContext)
        {
            _messages = mongoDatabase.GetCollection<MessageCollection>("messages");
            _hubContext = hubContext;
        }

        public async Task SendMessageAsync(MessageCollection message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", message.Sender, message.Content);
            await _messages.InsertOneAsync(message);
        }

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
