using MongoDB.Driver;
using TP_Messagerie.Data;

namespace TP_Messagerie.Services
{
    public class ConversationService
    {
        private readonly IMongoCollection<Conversation> _conversations;
        private readonly UserSession _userSession;

        public ConversationService(IMongoDatabase mongoDatabase, UserSession userSession)
        {
            _conversations = mongoDatabase.GetCollection<Conversation>("conversations");
            _userSession = userSession;
        }

        public async Task<List<Conversation>> GetConversationsForCurrentUserAsync()
        {
            // Recherche des conversations où le tableau participants contient le UserName
            return await _conversations
                .Find(c => c.Participants.Contains(_userSession.UserName))
                .ToListAsync();
        }

        public async Task UpdateLastMessageAsync(string lastMessage, string recipient)
        {
            // Les participants sont l'utilisateur actuel et le destinataire
            var participants = new[] { _userSession.UserName, recipient };

            // Construction du filtre pour vérifier si tous les participants sont présents
            var filter = Builders<Conversation>.Filter.All("participants", participants);

            // Définition des champs à mettre à jour
            var updateDefinition = Builders<Conversation>.Update
                .Set("lastMessage", lastMessage)
                .Set("lastUserMessage", _userSession.UserName)
                .Set("lastUpdated", DateTime.UtcNow)
                .SetOnInsert("participants", participants);

            // Effectue un upsert (mise à jour ou insertion)
            await _conversations.UpdateOneAsync(filter, updateDefinition, new UpdateOptions { IsUpsert = true });
        }
    }
}
