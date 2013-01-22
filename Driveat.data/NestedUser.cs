using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driveat.data
{
    /// <summary>
    /// A nested user is the representation of an user that can be embedded in a dish, in order to reduce requests to database by retreiving dishes only.
    /// [BsonIgnoreExtraElements] Ignore Elements that can be setted in the database without being defined in the representation model.
    /// [BsonRequiredAttribute] Defines a property needed in order store the entity in database.
    /// </summary>
    [BsonIgnoreExtraElements]
   public class NestedUser
    {
        /// <summary>
        /// Id of a Nested User.
        /// </summary>
       [BsonId]
        public ObjectId _id { get; set; }

        /// <summary>
        /// User email.
        /// </summary>
       [BsonRequiredAttribute]
       [BsonElement("email")]
       public string Email { get; set; }

        /// <summary>
        /// User name, cannot be modified.
        /// </summary>
       [BsonRequiredAttribute]
       [BsonElement("username")]
       public string Username { get; set; }
    }
}
