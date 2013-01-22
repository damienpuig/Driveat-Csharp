using Driveat.data;
using Driveat.Services.MongoNoRMService;
using MongoDB.Driver.Builders;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver.Linq;

namespace Driveat.Services.EnumServices
{
    /// <summary>
    /// Following the abstract factory pattern, Actions are disociated from the real service layer. We can have here the implementation of the 
    /// mongoDB section, but the interface can be used on another ORM ( Actually no because interfaces are not well done due to objects mapper issues).
    /// Each Entity has implementations specific to the mongoDB. 
    /// </summary>
    public class DishTypeService : IDishTypeService
    {
        /// <summary>
        /// dish type collection of mongoDB
        /// </summary>
        public readonly MongoHelper<DishType> _dishTypes;

        public DishTypeService()
        {
            //collection instance.
            _dishTypes = new MongoHelper<DishType>("dishtypes");
        }

        /// <summary>
        /// get the full dish type list.
        /// </summary>
        /// <returns>returns all the dish types.</returns>
        public List<data.DishType> GetDishTypes()
        {
          return  _dishTypes.Collection.FindAll().ToList();
        }

        /// <summary>
        /// Get dish type by name.
        /// </summary>
        /// <param name="dishTypeName">name of the dish type to be returned.</param>
        /// <returns>dish type returned.</returns>
        public data.DishType GetDishTypeByName(string dishTypeName)
        {
            //Find or default can return null.
            return _dishTypes.Collection.Find(Query.EQ("name", dishTypeName)).SingleOrDefault();
        }

        /// <summary>
        /// Get a dish by a dish type id ( used in a user choice, adding a dish ) 
        /// </summary>
        /// <param name="id">id of the selected dish type.</param>
        /// <returns>returns given dish type.</returns>
        public DishType GetDishTypeById(string id)
        {
            var objId = ObjectId.Parse(id);
            return _dishTypes.Collection.AsQueryable().Where(dish => dish._id == objId).FirstOrDefault();
        }
    }
}
