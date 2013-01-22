using Driveat.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driveat.Services.SearchService
{
    /// <summary>
    /// Search Service layer, containing the business of the application according to dishes.
    /// Each method accept data object.
    /// In a Real application, the use of mappers such as http://automapper.org/ is needed in order to perform transparency between layers.
    /// An object isn't shared between layers, but is mapped from Presentation view model to Data transfert object or command, and persisted to 
    /// the database as real entity objects.
    /// Dish types cannot be created in this coursework scope, because dish type are specific to the business and have to be defined only by
    /// administrators.
    /// </summary>
   public interface ISearchService
    {
       /// <summary>
       /// Search dishes by location and distance ( distance from 1 to 50 ).
       /// </summary>
       /// <param name="address">address given by the user, interpreted by the geoLocaliser Service.</param>
       /// <param name="scope">distance from the address to search.</param>
       /// <returns>returns the search result, can be null if dishes not found.</returns>
       IList<Dish> SearchDishesByLocationAndScope(string address, int scope);

       /// <summary>
       /// Search dishes by location, type and distance ( distance from 1 to 50 ).
       /// </summary>
       /// <param name="address">address given by the user, interpreted by the geoLocaliser Service.</param>
       /// <param name="dishType">dish type of the given entry.</param>
       /// <param name="scope">distance from the address to search.</param>
       /// <returns></returns>
       IList<Dish> SearchDishesByDishTypeAndLocation(string address, DishType dishType, int scope);
    }
}
