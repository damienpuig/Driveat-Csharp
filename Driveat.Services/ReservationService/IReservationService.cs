using Driveat.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driveat.Services.ReservationService
{
    /// <summary>
    /// Reservation Service layer, containing the business of the application according to dishes.
    /// Each method accept data object.
    /// In a Real application, the use of mappers such as http://automapper.org/ is needed in order to perform transparency between layers.
    /// An object isn't shared between layers, but is mapped from Presentation view model to Data transfert object or command, and persisted to 
    /// the database as real entity objects.
    /// Dish types cannot be created in this coursework scope, because dish type are specific to the business and have to be defined only by
    /// administrators.
    /// </summary>
    public interface IReservationService
    {
        /// <summary>
        /// Create a reservation.
        /// </summary>
        /// <param name="newReservation">accept a new reservation instance.</param>
        /// <returns>return the persisted reservation.</returns>
        Reservation CreateAReservation(Reservation newReservation);

        /// <summary>
        /// Get a reservation by it id. ( A string because the cast is only performed in the services layer. Presentation layer tends to deal
        /// only with string..)
        /// </summary>
        /// <param name="id">Reservation id</param>
        /// <returns>Returns the reservation</returns>
        Reservation GetReservationById(string id);

        /// <summary>
        /// Get sales by the seller id.
        /// </summary>
        /// <param name="userId">seller id.</param>
        /// <returns>returns a list of reservations by the seller id.</returns>
        List<Reservation> GetReservationBySellerId(string userId);

        /// <summary>
        /// Get reservations by the seller id.
        /// </summary>
        /// <param name="userId">buyer id.</param>
        /// <returns>returns a list of reservations by the buyer id.</returns>
        List<Reservation> GetReservationByBuyerId(string userId);

        /// <summary>
        /// Update a existing reservation.
        /// </summary>
        /// <param name="reservation">The update needs to be perfomed by giving the modified instance of the reservation.</param>
        /// <returns> returns the updated reservation</returns>
        Reservation UpdateReservation(Reservation reservation);
    }
}
