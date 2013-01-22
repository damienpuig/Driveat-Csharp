using Driveat.data;
using Driveat.Services.MongoNoRMService;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driveat.Services.DishService
{
    /// <summary>
    /// Following the abstract factory pattern, Actions are disociated from the real service layer. We can have here the implementation of the 
    /// mongoDB section, but the interface can be used on another ORM ( Actually no because interfaces are not well done due to objects mapper issues).
    /// Each Entity has implementations specific to the mongoDB. 
    /// </summary>
   public class DishService: IDishService
    {
       /// <summary>
        /// Specific implementation of the dish table, following the custom mongoDB collection mapper MongoHelper.
       /// </summary>
       public readonly MongoHelper<Dish> _dishes;

       public DishService()
       {
           //Instance of the collection.
           _dishes = new MongoHelper<Dish>("dishes");
       }

       /// <summary>
       /// Implementation of Get user by Id.
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        public data.Dish GetDishById(string id)
        {
            //Unsercure parse.
            var did = ObjectId.Parse(id);

            //Get a dish or null.
            var result = _dishes.Collection
                .AsQueryable()
                .Where(dish => dish._id == did)
                .FirstOrDefault();

            return result;
        }

       /// <summary>
       /// Get dishes list by a given user id
       /// </summary>
       /// <param name="userId">user id</param>
       /// <returns>List or null.</returns>
        public List<Dish> GetDishesByUserId(string userId)
        {
            //Unsercure parse.
            var userObjId = ObjectId.Parse(userId);
            
            return _dishes.Collection.AsQueryable().Where(dish => dish.Seller._id == userObjId).ToList();
        }

       //Update of a given dish.
        public data.Dish UpdateDish(data.Dish dish)
        {
            //Query defines the parameter
            var query = Query.EQ("_id", dish._id);
            _dishes.Collection.Update(query, Update.Replace<Dish>(dish));
            return dish;
        }

       /// <summary>
       /// Create a dish by giving a new dish instance.
       /// </summary>
       /// <param name="newDish"> the new dish to be saved.</param>
       /// <returns>returns the current dish or null.</returns>
        public Dish CreateADish(Dish newDish)
        {
            var result  = _dishes.Collection.Insert(newDish);
            if (result.Ok)
                return newDish;

            return null;
        }

       /// <summary>
       /// Delete a dish by giving the dish id.
       /// </summary>
       /// <param name="id">id of the dish to be delete.</param>
       /// <returns>returns state of the delete action</returns>
        public bool DeleteDish(string id)
        {
            var objId = ObjectId.Parse(id); 
            var query = Query.EQ("_id", objId);
            var result = _dishes.Collection.Remove(query);

           if (result.Ok)
               return true;

           return false;
            
        }
    }
}
