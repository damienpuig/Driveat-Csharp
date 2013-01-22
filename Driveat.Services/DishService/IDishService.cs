using Driveat.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driveat.Services.DishService
{
    /// <summary>
    /// Dish Service layer, containing the business of the application according to dishes.
    /// Each method accept data object.
    /// In a Real application, the use of mappers such as http://automapper.org/ is needed in order to perform transparency between layers.
    /// An object isn't shared between layers, but is mapped from Presentation view model to Data transfert object or command, and persited to 
    /// the database as real entity objects.
    /// </summary>
    public interface IDishService
    {
        /// <summary>
        /// Creation of a dish, accepting in parameter a dish.
        /// </summary>
        /// <param name="newDish">New dish.</param>
        /// <returns>Returns the created dish.</returns>
        Dish CreateADish(Dish newDish);

        /// <summary>
        /// Retreive a dish by a given id.
        /// </summary>
        /// <param name="id"> id to retreive the exact dish.</param>
        /// <returns>dish found. </returns>
        Dish GetDishById(string id);

        /// <summary>
        /// Get a dishes list by it given user id.
        /// </summary>
        /// <param name="userId">given user id</param>
        /// <returns>If the result is null, the value null is returned</returns>
        List<Dish> GetDishesByUserId(string userId);

        /// <summary>
        /// Update a dish given to the modified dish.
        /// </summary>
        /// <param name="dish">the modified dish</param>
        /// <returns>the dish persisted ( actually the same as the dish persisted earlier.)</returns>
        Dish UpdateDish(Dish dish);

        /// <summary>
        /// Delete a dish by it id.
        /// </summary>
        /// <param name="id">id of the dish to by deleted.</param>
        /// <returns></returns>
        bool DeleteDish(string id);
    }
}
