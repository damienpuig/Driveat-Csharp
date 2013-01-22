using Driveat.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driveat.Services.EnumServices
{
    /// <summary>
    /// DishType Service layer, containing the business of the application according to dishes.
    /// Each method accept data object.
    /// In a Real application, the use of mappers such as http://automapper.org/ is needed in order to perform transparency between layers.
    /// An object isn't shared between layers, but is mapped from Presentation view model to Data transfert object or command, and persisted to 
    /// the database as real entity objects.
    /// Dish types cannot be created in this coursework scope, because dish type are specific to the business and have to be defined only by
    /// administrators.
    /// </summary>
    public interface IDishTypeService
    {
        /// <summary>
        /// Get a list a dish types given by the business.
        /// </summary>
        /// <returns>returns the actual list of dish types.</returns>
        List<DishType> GetDishTypes();

        /// <summary>
        /// Get a dish type by name.
        /// </summary>
        /// <param name="dishTypeName">name of the type.</param>
        /// <returns>dish type returned according to it name.</returns>
        DishType GetDishTypeByName(string dishTypeName);

        /// <summary>
        /// Get a dishtype (Used the characterize a dish).
        /// </summary>
        /// <param name="id">Id of a dish type.</param>
        /// <returns>returns the current dish type.</returns>
        DishType GetDishTypeById(string id);
    }
}
