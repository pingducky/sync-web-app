using MongoDB.Bson;
using MongoDB.Driver;
using TP_Messagerie.Data;

namespace TP_Messagerie.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("users");
        }

        public async Task CreateUserAsync(User user) =>
            await _users.InsertOneAsync(user);

        public async Task DeleteUserAsync(string id) =>
            await _users.DeleteOneAsync(u => u.Id == id);

        public async Task UpdateUserAsync(User user) =>
            await _users.ReplaceOneAsync(u => u.Id == user.Id, user);

        public async Task<User?> GetUserByIdAsync(string id) =>
            await _users.Find(u => u.Id == id).FirstOrDefaultAsync();

        public async Task<User?> GetUserByUsernameAsync(string username) =>
            await _users.Find(u => u.Username == username).FirstOrDefaultAsync();

        public async Task<List<User>?> GetAllUsersAsync() =>
            await _users.Find(_ => true).ToListAsync();

        public async Task<User?> LoginAsync(string username, string password) =>
            await _users.Find(u => u.Username == username && u.Password == password).FirstOrDefaultAsync();
    }
}
