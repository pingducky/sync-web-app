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
            return await _conversations
                .Find(c => c.Participants.Contains(_userSession.UserName))
                .ToListAsync();
        }

        public async Task UpdateLastMessageAsync(string lastMessage, string recipient)
        {
            // Les participants sont l'utilisateur actuel et le destinataire
            var participants = new List<string> { _userSession.UserName, recipient };


            // Construction du filtre pour vérifier si tous les participants sont présents
            var filter = Builders<Conversation>.Filter.All("participants", participants);

            // Vérifiez si une conversation existe déjà
            var existingConversation = await _conversations.Find(filter).FirstOrDefaultAsync();

            if (existingConversation != null)
            {
                // Mise à jour des champs pour une conversation existante
                var updateDefinition = Builders<Conversation>.Update
                    .Set("lastMessage", lastMessage)
                    .Set("lastUserMessage", _userSession.UserName)
                    .Set("lastUpdated", DateTime.UtcNow);

                await _conversations.UpdateOneAsync(filter, updateDefinition);
            }
            else
            {
                // Création d'un nouveau document si la conversation n'existe pas
                var newConversation = new Conversation
                {
                    Participants = participants,
                    LastMessage = lastMessage,
                    LastUserMessage = _userSession.UserName,
                    LastUpdated = DateTime.UtcNow
                };

                await _conversations.InsertOneAsync(newConversation);
            }

        }
    }
}
