using Driveat.data;
using Driveat.Services.MongoNoRMService;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Driveat.Services.UserService
{
    /// <summary>
    /// User Service layer, containing the business of the application according to dishes.
    /// Each method accept data object.
    /// In a Real application, the use of mappers such as http://automapper.org/ is needed in order to perform transparency between layers.
    /// An object isn't shared between layers, but is mapped from Presentation view model to Data transfert object or command, and persisted to 
    /// the database as real entity objects.
    /// Dish types cannot be created in this coursework scope, because dish type are specific to the business and have to be defined only by
    /// administrators.
    /// </summary>
   public class UserService : IUserService
    {
       //User mongoDB collection
        public readonly MongoHelper<User> _Users;

       /// <summary>
       /// Bson document representing a new user entry.
       /// Initialy used in order to perform request on the database, this model can be 
       /// changed by an User entity.
       /// </summary>
        public BsonDocument Login { get; set; }

        public UserService()
        {
            //User mongoDB collection instance.
            _Users = new MongoHelper<User>("users", "email", "username");
        }

        public data.User GetUserByEmail(string email)
        {
          return  _Users.Collection.AsQueryable().Where(user => user.Email == email).FirstOrDefault();
        }

        public data.User UpdateUser(User user)
        {
            var query = Query.EQ("_id", user._id);
            _Users.Collection.Update(query, Update.Replace<User>(user));
            return user;
        }


        public User GetUserByUsername(string username)
        {
            return _Users.Collection.AsQueryable().Where(user => user.Username == username).FirstOrDefault();
        }


        public User CreateUser(string username, string email, string password)
        {
            var userpresent = _Users.Collection.AsQueryable().Where(user => (user.Username == username) || (user.Email == email)).FirstOrDefault();

            if (userpresent != null)
                return null;

            //New bCrypt salt.
            var salt = BCrypt.Net.BCrypt.GenerateSalt();

            var newUser = new BsonDocument
            {
                { "email", email },
                { "username", username },
                { "password_hash", this.EncodePassword(password, salt) },
                { "password_salt", salt }
            };

            //User insertion in database.
            _Users.Collection.Insert(newUser);
            return GetUserByUsername(username);
        }


        public User ValidateUser(string email, string password)
        {
            var query = Query.EQ("email", email);

            var user = _Users.Collection.FindOne(query);

            if (user == null)
            {
                return null;
            }

            if (this.VerifyPassword(user, password))
            {
                return GetUserByEmail(email);
            }

            return null;
        }

       //Check if a password matches it salt.
        private bool VerifyPassword(User user, string password)
        {
            return user.Password_Hash == EncodePassword(password, user.Password_Salt);
        }

        private string EncodePassword(string password, string salt)
        {


            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(salt))
            {
                return null;
            }
            //Password hash action
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }



        public User GetUserById(string id)
        {
            var objId = ObjectId.Parse(id);
            return _Users.Collection.AsQueryable().Where(user => user._id == objId).FirstOrDefault();
        }
    }
}
