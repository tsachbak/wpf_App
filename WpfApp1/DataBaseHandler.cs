using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class DataBaseHandler
    {
        private readonly IMongoCollection<User> _userCollection;
        private IMongoDatabase _db;

        public DataBaseHandler()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            _db = client.GetDatabase("mydatabase");
            _userCollection = _db.GetCollection<User>("users");
        }

        public void InsertUser(User user)
        {
            _userCollection.InsertOne(user);
        }

        public void DeleteUser(User user)
        {
            //string userID = _userCollection
        }

        public List<User> GetUsersFromDB() 
        {
            return _userCollection.Find(_ => true).ToList<User>();
        }
    }
}
