using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;


namespace Driveat.data
{
    /// <summary>
    /// Object representation of a dish. The instanciated object is linked to an mongoDB object representation.
    /// [BsonRequiredAttribute] Defines a property needed in order store the entity in database.
    /// [BsonIgnoreIfNull] Defines a property not needed by the business.
    /// </summary>
    public class Dish
    {
        //Dish constructor with parameter if needed
       public Dish(string name, double price, DateTime availability)
       {
           Name = name;
           Price = price;
           Availability = availability;
       }

        //Default constructor mandatory if parametized constructor defined.
       public Dish()
       {

       }


        /// <summary>
        /// Dish id, in ObjectId matching database type. required by default in mongoDB
        /// </summary>
       [BsonId]
       public ObjectId _id { get; set; }

        /// <summary>
        /// Dish name. 
        /// </summary>
        [BsonRequiredAttribute]
        [BsonElement("name")]
        public string Name { get; set; }

        /// <summary>
        /// Price of a dish, between 2 and 10, restricted by the website.
        /// </summary>
        [BsonRequiredAttribute]
        [BsonElement("price")]
        public double Price { get; set; }

        /// <summary>
        /// Availability of a dish. A dish can be booked until the given date.
        /// </summary>
        [BsonRequiredAttribute]
        [BsonElement("availability")]
        public DateTime Availability { get; set; }

        /// <summary>
        /// Content of the given dish. 
        /// </summary>
       [BsonRequiredAttribute]
       [BsonElement("food")]
        public string Food { get; set; }

        /// <summary>
        /// Description of the dish.
        /// </summary>
       [BsonRequiredAttribute]
       [BsonElement("description")]
        public string Description { get; set; }

        /// <summary>
        /// Type of the dish, defined by a DishType, stored in database. This type is an embedded type.
        /// </summary>
       [BsonRequiredAttribute]
       [BsonElement("dishtype")]
       public DishType Dishtype { get; set; }

        /// <summary>
        /// Nested person ( only _id, Username and email). Those parameters are embedded in the dish because there are 
        /// unique and cannot be changed by the user. ( Even in the User entity )
        /// </summary>
        [BsonRequiredAttribute]
        [BsonElement("nesteduser")]
        public NestedUser Seller { get; set; }

        /// <summary>
        /// Picture filename. Picture are stored in COMMON/dish/FILENAME
        /// </summary>
        [BsonIgnoreIfNull]
        [BsonElement("picture_filename")]
        public string Picture { get; set; }
    }


}
