using MongoDB.Bson;
using MongoDB.Driver;


namespace WpfApp1
{
    public  class AppContext
    {
        static readonly string connectionString = "mongodb+srv://Young_Abobus:y0_466_o@salesmantrip.bqcxu.mongodb.net/myFirstDatabase?retryWrites=true&w=majority";

        public static void SaveNewUser(User newUser)
        {
            MongoClient client = new MongoClient(connectionString);
            var database = client.GetDatabase("SalesmanTrip");
            var collection = database.GetCollection<BsonDocument>("Users");
            BsonDocument user = new BsonDocument
                {
                    {"userLogin", newUser.userLogin },
                    {"userPassword", newUser.userPassword }
                };

            collection.InsertOne(user);

        }
        //по логину
        public static bool FindUserByLogin(string login)
        {
            MongoClient client = new MongoClient(connectionString);
            var database = client.GetDatabase("SalesmanTrip");
            var collection = database.GetCollection<BsonDocument>("Users");
            var filter = new BsonDocument("userLogin", login);

            return collection.Find(filter).CountDocuments() > 0;

        }
        //по паролю
        public static bool FindUserByLoginAndPass(string login, string password)
        {
            MongoClient client = new MongoClient(connectionString);
            var database = client.GetDatabase("SalesmanTrip");
            var collection = database.GetCollection<BsonDocument>("Users");
            var builder = Builders<BsonDocument>.Filter;

            var filter = builder.Eq("userLogin", login) & builder.Eq("userPassword", password);
            return collection.Find(filter).CountDocuments() > 0;

        }
    }
}
