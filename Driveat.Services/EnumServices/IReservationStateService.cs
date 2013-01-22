using Driveat.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driveat.Services.EnumServices
{
    public interface IReservationStateService
    {
        List<ReservationState> getReservationStates();
        ReservationState GetReservationStatesByName(string ReservationStateName);
        ReservationState CreateReservationState(ReservationState newReservationState);
    }
}
