using Driveat.data;
using Driveat.Services.MongoNoRMService;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driveat.Services.ReservationService
{
    /// <summary>
    /// Following the abstract factory pattern, Actions are disociated from the real service layer. We can have here the implementation of the 
    /// mongoDB section, but the interface can be used on another ORM ( Actually no because interfaces are not well done due to objects mapper issues).
    /// Each Entity has implementations specific to the mongoDB. 
    /// </summary>
    public class ReservationService : IReservationService
    {
        /// <summary>
        /// Reservation collection property given by the MongoHelper.
        /// </summary>
        public readonly MongoHelper<Reservation> _reservations;


        public ReservationService()
        {
            //Reservation instance.
            _reservations = new MongoHelper<Reservation>("reservations");
        }

        public Reservation CreateAReservation(Reservation newReservation)
        {
            //A reservation creation is always defined by the state call to confirm to the seller.
            newReservation.State = new ReservationState() { Name = ReservationState.State.CallintoConfirm.Value() };
            var result = _reservations.Collection.Insert(newReservation);
            if (result.Ok)
                return newReservation;

            return null;
        }

        public Reservation GetReservationById(string id)
        {
            var resObjectId = ObjectId.Parse(id);
            return _reservations.Collection.AsQueryable().Where(reservation => reservation._id == resObjectId).FirstOrDefault();
        }

        public List<Reservation> GetReservationBySellerId(string userId)
        {
            var userObjectId = ObjectId.Parse(userId);
            return _reservations.Collection.AsQueryable().Where(reservation => reservation.LinkedDish.Seller._id == userObjectId).ToList();
        }

        public List<Reservation> GetReservationByBuyerId(string userId)
        {
            var userObjectId = ObjectId.Parse(userId);
            return _reservations.Collection.AsQueryable().Where(reservation => reservation.Buyer._id == userObjectId).ToList();
        }

        //Update done on the reservation id of the given reservation.
        public Reservation UpdateReservation(Reservation reservation)
        {
            var query = Query.EQ("_id", reservation._id);
            _reservations.Collection.Update(query, Update.Replace<Reservation>(reservation));
            return reservation;
        }
    }
}
