using Driveat.data;
using Driveat.Services.MongoNoRMService;
using GeoCoding;
using GeoCoding.Google;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using MongoDB.Bson;

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
    public class SearchService : ISearchService
    {
        //Dish collection property.
        public readonly MongoHelper<Dish> _dishes;

        //User collection property
        public readonly MongoHelper<User> _users;

        // Geocode service performing mapping between user entry and location result. 
        //(The search is based on this service only)..
        //The search is based on the google service.
        public readonly IGeoCoder _geocode;

        //Function used by the search to search within the lINQ query.
        private Func<User, Address, bool> dishinmiles { get; set; }

        //Scope defined by the user to search
        public int Scope { get; set; }


        public SearchService()
        {
            _dishes = new MongoHelper<Dish>("dishes");
            _users = new MongoHelper<User>("users");
            _geocode = new GoogleGeoCoder();

            //takes 2 parameters: 
            //the current user given by the linq query ( selector )
            //the address given by the user performing the search, instanciated as a Geocoding.Address type.
            //returns true if the distance between the current user selector and the given address is less than the distance scope,
            // otherwise, false.
            dishinmiles = (user, addressrequested) =>   
                {
                  return _geocode
                   .ReverseGeocode(user.Location.Coordinates[1], user.Location.Coordinates[0])
                   .FirstOrDefault()
                   .DistanceBetween(addressrequested, DistanceUnits.Miles).Value < Scope;
                };
        }

        /// <summary>
        /// Search a dish list from a given address and a distance
        /// </summary>
        /// <param name="address">user address request</param>
        /// <param name="scope">distance request</param>
        /// <returns>returns a dish list or null</returns>
        public IList<Dish> SearchDishesByLocationAndScope(string address, int scope)
        {
            var availableUsers = getAvailableUsers(address, scope);

            var result = _dishes.Collection.AsQueryable()
                        .Where(dish => dish.Seller._id.In(availableUsers))
                        .ToList();

            return result;
        }

        /// <summary>
        /// Search a dish list from a given address, a dishtype and a scope.
        /// </summary>
        /// <param name="address">address given by the user, interpreted by the geoLocaliser Service.</param>
        /// <param name="dishType">dish type of the given entry.</param>
        /// <param name="scope">distance from the address to search.</param>
        /// <returns>returns a dish list or null</returns>
        public IList<Dish> SearchDishesByDishTypeAndLocation(string address, DishType dishType, int scope)
        {
            var availableUsers = getAvailableUsers(address, scope);

            var result = _dishes.Collection.AsQueryable()
                        .Where(dish => dish.Dishtype.Name == dishType.Name)
                        .Where(dish => availableUsers.Contains(dish.Seller._id))
                        .ToList();

            return result;
        }

        /// <summary>
        /// Shared method searching on users, with a distance and a address.
        /// </summary>
        /// <param name="address">address given by the user, interpreted by the geoLocaliser Service.</param>
        /// <param name="scope">distance from the address to search.</param>
        /// <returns>returns a id list only, in order to be user on the dish collection.</returns>
        private IList<ObjectId> getAvailableUsers(string address, int scope)
        {
            Scope = scope;

            //If scope is more than 100 (security), set the scope to 10.
            if (scope > 100)
                Scope = 10;

            var addressresult = _geocode.GeoCode(address).FirstOrDefault();

            var queryOnU = from user in _users.Collection.FindAll()
                           where user.Location != null
                           where dishinmiles.Invoke(user, addressresult)
                           select  user._id;

            return queryOnU.ToList();
        }
    }
}
