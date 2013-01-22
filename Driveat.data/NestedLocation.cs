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
    /// Representation of a location in database. This model is not stored in a table, but embedded in a user ( Nested location ).
    /// [BsonRequiredAttribute] Defines a property needed in order store the entity in database.
    /// </summary>
   public class NestedLocation
    {
        [BsonId]
        public ObjectId _id { get; set; }

       /// <summary>
        /// Longitude = [0]
       /// Latitude = [1]
       /// Coordinates setted by a GeoService.
       /// </summary>
        [BsonRequiredAttribute]
        [BsonElement("coordinates")]
        public double[] Coordinates { get; set; }

       /// <summary>
       /// Address setted by a Geo service.
       /// </summary>
        [BsonRequiredAttribute]
        [BsonElement("address")]
        public string Address { get; set; }
    }
}
