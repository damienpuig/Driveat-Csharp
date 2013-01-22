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
    /// Representation of a reservation. A Reservation is only based on embedded models, because a reservation cannot be changed by foreign associations.
    /// Each custon type needs to be persisted.
    /// [BsonRequiredAttribute] Defines a property needed in order store the entity in database.
    /// </summary>
     [BsonIgnoreExtraElements]
    public class Reservation
    {
        /// <summary>
        /// Object representation of a reservation. The instanciated object is linked to an mongoDB object representation
        /// </summary>
        [BsonId]
        public ObjectId _id { get; set; }

         /// <summary>
         /// User buying the dish.
         /// </summary>
        [BsonRequiredAttribute]
        [BsonElement("buyer")]
        public NestedUser Buyer { get; set; }

         /// <summary>
         /// Date given by the buyer.
         /// </summary>
        [BsonRequiredAttribute]
        [BsonElement("date")]
        public DateTime Date { get; set; }

         /// <summary>
         /// Hour given by the buyer.
         /// </summary>
        [BsonRequiredAttribute]
        [BsonElement("hour")]
        public DateTime Hour { get; set; }

         /// <summary>
         /// Dish linked to the reservation. The dish is persisted in the reservation in the reservation state.
         /// </summary>
        [BsonRequiredAttribute]
        [BsonElement("dishconcerned")]
        public Dish LinkedDish { get; set; }

         /// <summary>
         /// Business state of the reservation.
         /// </summary>
        [BsonRequiredAttribute]
        [BsonIgnoreIfNull]
        [BsonElement("state")]
        public ReservationState State { get; set; }
    }
}
