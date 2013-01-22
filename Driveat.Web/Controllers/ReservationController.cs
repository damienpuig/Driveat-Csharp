using Driveat.data;
using Driveat.Services.DishService;
using Driveat.Services.EnumServices;
using Driveat.Services.ReservationService;
using Driveat.Services.UserService;
using Driveat.Web.Models;
using MongoDB.Bson;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Driveat.Services;

namespace Driveat.Web.Controllers
{
    [Authorize]
    public class ReservationController : BaseController
    {

        public IReservationService ReservationSvc { get; set; }
        public IUserService UserSvc { get; set; }
        public IDishService DishSvc { get; set; }

        //NInject attribute to inject services in the contructor. Controllers are independent from service layer.
        [Inject]
        public ReservationController(IReservationService reservationservice, IUserService userservice, IDishService dishservice)
        {
            ReservationSvc = reservationservice;
            UserSvc = userservice;
            DishSvc = dishservice;
        }

        /// <summary>
        /// One way action ( Only post ) from Dish/Show
        /// </summary>
        /// <param name="date">date of the meeting, depending on the availability dish date.</param>
        /// <param name="hour">hour of the meeting.</param>
        /// <param name="dishId">given dish id.</param>
        /// <returns>return to Account/Index OR Dish/Show</returns>
        [HttpPost]
        public ActionResult Reserve(string date, string hour, string dishId)
        {
            //safe datetime parsing
            DateTime givenDate;
            DateTime givenHour;
            DateTime.TryParse(date, out givenDate);
            DateTime.TryParse(hour, out givenHour);


            //If wrong date or hour, return to the page.
            if (givenDate == null || givenHour == null)
            {
                return RedirectToAction("Show", "Dish", new { dishid = dishId });
            }


            var User = UserContext.Current.CurrentUser;
            var dish = DishSvc.GetDishById(dishId);

            //If the user is the sqme thqn the dish, returns to Dish/Show notifying the error.
            if (User.id == dish.Seller._id.ToString())
            {
                UserContext.Current.Notify = "Error! You cannot reserve your own dish!";
                UserContext.Current.NotifyType = notificationType.error.Value();
                return RedirectToAction("Show", "Dish", new { dishid = dishId });
            }


            // new reservation
                var newReservation = new Reservation()
                {
                    Buyer = new NestedUser() { _id = ObjectId.Parse(User.id), Email = User.Email, Username = User.Username },
                    Date = givenDate,
                    Hour = givenHour,
                    LinkedDish = dish
                };

            // The call to confirm state is setted within the service.
                ReservationSvc.CreateAReservation(newReservation);

            //notification
                UserContext.Current.Notify = "Reservation done! Check out reservation category.";
                UserContext.Current.NotifyType = notificationType.success.Value();

                return RedirectToAction("Index", "Account");
        }


        //Set the decline status to the reservation.
        public ActionResult Decline(string id)
        {
            //Checks if the reservation is retreivable
            var reservation = CheckValidity(id);

            //redirects if the reservation goes in trouble.
            if (reservation == null)
                return RedirectToAction("Index", "Account");

            //set the Declined statut to the reservation.
            reservation.State = new ReservationState() { Name = ReservationState.State.Declined.Value() };

            //Update the reservation
            ReservationSvc.UpdateReservation(reservation);

            //notify
            UserContext.Current.Notify = "Reservation declined!";
            UserContext.Current.NotifyType = notificationType.success.Value();

            return RedirectToAction("Index", "Account");
        }

        //Cancel a reservation in the same way as decline
        public ActionResult Cancel(string id)
        {
            var reservation = CheckValidity(id);

            if (reservation == null)
                return RedirectToAction("Index", "Account");

            reservation.State = new ReservationState() { Name = ReservationState.State.Canceled.Value() };
            ReservationSvc.UpdateReservation(reservation);

            UserContext.Current.Notify = "Reservation canceled!";
            UserContext.Current.NotifyType = notificationType.success.Value();

            return RedirectToAction("Index", "Account");
        }

        //Reconsiders a reservation by setting the call into confirm status. 
        public ActionResult Reconsider(string id)
        {
            var reservation = CheckValidity(id);

            if (reservation == null)
                return RedirectToAction("Index", "Account");

            reservation.State = new ReservationState() { Name = ReservationState.State.CallintoConfirm.Value() };
            ReservationSvc.UpdateReservation(reservation);

            UserContext.Current.Notify = "Reservation reconsidered!";
            UserContext.Current.NotifyType = notificationType.success.Value();

            return RedirectToAction("Index", "Account");
        }


        //Cancel a reservation in the same way as Cancel
        public ActionResult Approve(string id)
        {
            var reservation = CheckValidity(id);

            if (reservation == null)
                return RedirectToAction("Index", "Account");

            reservation.State = new ReservationState() { Name = ReservationState.State.Approved.Value() };
            ReservationSvc.UpdateReservation(reservation);

            UserContext.Current.Notify = "Reservation Approved!";
            UserContext.Current.NotifyType = notificationType.success.Value();

            return RedirectToAction("Index", "Account");
        }

        //Common method checking if the reservation is retreivable.
        //if the reservation is not retreivable, notifying.
        public Reservation CheckValidity(string id)
        {
            var reservation = ReservationSvc.GetReservationById(id);

            if (reservation == null)
            {
                UserContext.Current.Notify = "A Problem came while updating the reservation";
                UserContext.Current.NotifyType = notificationType.error.Value();
                return null;
            }

            return reservation;
        }



    }
}
