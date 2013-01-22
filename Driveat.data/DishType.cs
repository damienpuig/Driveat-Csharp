using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driveat.data
{    
    /// <summary>
    /// Object representation of a dish type. The instanciated object is linked to an mongoDB object representation
    /// [BsonRequiredAttribute] Defines a property needed in order store the entity in database.
    /// </summary>
    public class DishType
    {
        public DishType(string name)
        {
            Name = name;
        }

        public DishType()
        {

        }

        /// <summary>
        /// Id of a dishType, defined by a ObjectId.
        /// </summary>
        [BsonId]
        public ObjectId _id { get; set; }

        /// <summary>
        /// Name of a dishType.
        /// </summary>
        [BsonRequiredAttribute]
        [BsonElement("name")]
        public string Name { get; set; }
    }
}
