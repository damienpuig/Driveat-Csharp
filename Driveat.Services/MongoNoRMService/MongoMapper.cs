using Driveat.data;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driveat.Services.MongoNoRMService
{
    public class MongoMapper
    {
            public static void Configure()
            {
                BsonClassMap.RegisterClassMap<Dish>(cm => cm.AutoMap());
                BsonClassMap.RegisterClassMap<DishType>(cm => cm.AutoMap());
                BsonClassMap.RegisterClassMap<Reservation>(cm => cm.AutoMap());
                BsonClassMap.RegisterClassMap<ReservationState>(cm => cm.AutoMap());
                BsonClassMap.RegisterClassMap<User>(cm => cm.AutoMap());
            }
    }
}
