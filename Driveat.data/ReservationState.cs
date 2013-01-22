using Driveat.Data;
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
    /// Object representation of a reservation state. The instanciated object is linked to an mongoDB object representation.
    /// [BsonRequiredAttribute] Defines a property needed in order store the entity in database.
    /// </summary>
    public class ReservationState
    {
        public ReservationState()
        {

        }

        /// <summary>
        /// Business state of the reservation are not persited in database ( And that is a big mistake, because Business states was previously stored
        /// but I made an error persisting it into the business representation model data layer.
        /// I choosed to persist it in the data layer because there is no point that a client can change the business workflow of an application
        /// on the fly.
        /// Each type string value can be accessed by the .Value extension linked to enums, with: using Driveat.Services)
        /// </summary>
        public enum State
        {
            [StringValue("Approved")]
            Approved,
            [StringValue("Declined")]
            Declined,
            [StringValue("Call in to confirm")]
            CallintoConfirm,
            [StringValue("Canceled")]
            Canceled,
            [StringValue("Payment Confirmed")]
            PaymentConfirmed
        };

        /// <summary>
        /// Reservation state id.
        /// </summary>
       [BsonId]
        public ObjectId _id { get; set; }

        /// <summary>
        /// Business state name of the reservation.
        /// </summary>
        [BsonRequiredAttribute]
        [BsonElement("name")]
        public string Name { get; set; }
    }
}
