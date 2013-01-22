using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Driveat.data
{
    /// <summary>
    /// Representation model of a dish.
    /// [BsonIgnoreExtraElements] Ignore Elements that can be setted in the database without being defined in the representation model.
    /// [BsonRequiredAttribute] Defines a property needed in order store the entity in database.
    /// [BsonIgnore] Defines a property not taken in account on the database object serialization processed by mongoDB.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class User
    {
        /// <summary>
        /// Object representation of an user. The instanciated object is linked to an mongoDB object representation
        /// </summary>
        [BsonId]
        public ObjectId _id { get; set; }

        /// <summary>
        /// Emil of a the user
        /// </summary>
        [BsonRequiredAttribute]
        [BsonElement("email")]
        public string Email { get; set; }

        /// <summary>
        /// Username of the user.
        /// </summary>
         [BsonRequiredAttribute]
        [BsonElement("username")]
        public string Username { get; set; }

        /// <summary>
        /// Firstname of the user.
         /// Not needed at the registration.
        /// </summary>
        [BsonIgnoreIfNull]
        [BsonElement("firstname")]
         public string Firstname { get; set; }

        /// <summary>
        /// Lastname of the user.
        /// Not needed at the registration.
        /// </summary>
        [BsonIgnoreIfNull]
        [BsonElement("lastname")]
        public string Lastname { get; set; }

        /// <summary>
        /// Information related to the user.
        /// Not needed at the registration.
        /// </summary>
        [BsonIgnoreIfNull]
        [BsonElement("aboutyou")]
        public string AboutYou { get; set; }

        /// <summary>
        /// Dishes related to the user.
        /// </summary>
        [BsonIgnore]
        public List<Dish> Dishes { get; set; }

        /// <summary>
        /// Reservation done by the user.
        /// </summary>
        [BsonIgnore]
        public List<Reservation> Reservations { get; set; }

        /// <summary>
        /// Sales performed by the user.
        /// </summary>
        [BsonIgnore]
        public List<Reservation> Sales { get; set; }

        /// <summary>
        /// Password salt.
        /// </summary>
        [BsonRequiredAttribute]
        [BsonElement("password_salt")]
        public string Password_Salt { get; set; }

        /// <summary>
        /// User password, hashed by the UserService.
        /// </summary>
        [BsonRequiredAttribute]
        [BsonElement("password_hash")]
        public string Password_Hash { get; set; }

        /// <summary>
        /// Nested location. The user is only searchable if the location is setted ( not mentioned by the business report... )
        /// </summary>
         [BsonIgnoreIfNull]
        [BsonElement("location")]
        public NestedLocation Location { get; set; }

        /// <summary>
        /// Optional picture of the user.
        /// </summary>
         [BsonIgnoreIfNull]
         [BsonElement("picture_filename")]
         public string Picture { get; set; }
    }
}
