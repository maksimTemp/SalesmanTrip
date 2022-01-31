using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WpfApp1
{
    public class User
    {
        [BsonId]
        [BsonElement("userLogin")]
        public string userLogin { get; set; }
        [BsonElement("userPassword")]
        public string userPassword { get; set; }
        public User() { }
        public User( string login, string password)
        {
            userLogin = login;
            userPassword = password;
        }
    }
}
