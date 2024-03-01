using MongoDB.Driver;
using UserProfileService.Database;
using UserProfileService.Models;
namespace UserProfileService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<Users> Users;
        public UserRepository(IDatabaseSettings settings) 
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            Users = database.GetCollection<Users>(settings.CollectionName);
        }
        public async Task<Users> GetUserById(string id)
        {
            var user=await Users.Find(x => x.UserId == id).FirstOrDefaultAsync();
            return user;
        }
        public async Task<Users> GetUserByUserName(string userName)
        {
            var user=await Users.Find(x => x.UserName == userName).FirstOrDefaultAsync();
            return user;
        }
        public async Task<Users> RegisterUser(Users user)
        {
             await Users.InsertOneAsync(user);
            return  user;
        }
    }
}
