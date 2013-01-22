using Driveat.data;
using Driveat.Services.MongoNoRMService;
using Driveat.Services;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driveat.Services.EnumServices
{
    public class ReservationStateService : IReservationStateService
    {
        public readonly MongoHelper<ReservationState> _ReservationTypes;

        public ReservationStateService()
        {
            _ReservationTypes = new MongoHelper<ReservationState>("reservationstate");
        }


        public List<data.ReservationState> getReservationStates()
        {
            return _ReservationTypes.Collection.FindAll().ToList();
        }

        public data.ReservationState GetReservationStatesByName(string ReservationStateName)
        {
            return _ReservationTypes.Collection.Find(Query.EQ("name", ReservationStateName)).SingleOrDefault();
        }


        public data.ReservationState CreateReservationState(data.ReservationState newReservationState)
        {
            var isAlreadyPresent = _ReservationTypes.Collection.FindOne(Query.EQ("name", newReservationState.Name));

            if (isAlreadyPresent != null)
                return isAlreadyPresent;

            var result = _ReservationTypes.Collection.Insert(newReservationState);
            if (result.Ok)
                return newReservationState;

            return null;
        }
    }
}
