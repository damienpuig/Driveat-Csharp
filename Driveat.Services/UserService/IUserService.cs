using Driveat.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Driveat.Services.UserService
{
    /// <summary>
    /// User Service layer, containing the business of the application according to dishes.
    /// Each method accept data object.
    /// In a Real application, the use of mappers such as http://automapper.org/ is needed in order to perform transparency between layers.
    /// An object isn't shared between layers, but is mapped from Presentation view model to Data transfert object or command, and persisted to 
    /// the database as real entity objects.
    /// Dish types cannot be created in this coursework scope, because dish type are specific to the business and have to be defined only by
    /// administrators.
    /// The user service is used to register, log, and get users.
    /// In order to log an user, the service uses the Bcrypt library.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Get the user by email address
        /// </summary>
        /// <param name="email">email of the user to be retreived</param>
        /// <returns>returns the associated user.</returns>
        User GetUserByEmail(string email);

        /// <summary>
        /// Get the user by username
        /// </summary>
        /// <param name="username">Username of the user to be retreived</param>
        /// <returns>returns the associated user.</returns>
        User GetUserByUsername(string username);

        /// <summary>
        /// Get the user by Id
        /// </summary>
        /// <param name="id">Id of the user to be retreived.</param>
        /// <returns>returns the associated user.</returns>
        User GetUserById(string id);

        /// <summary>
        /// Update the given user
        /// </summary>
        /// <param name="user">User to be updated.</param>
        /// <returns>Returns the updated user.</returns>
        User UpdateUser(User user);

        /// <summary>
        /// Create an user, with a username, email and password.
        /// if the user already exists, null is returned.
        /// </summary>
        /// <param name="username">Username of the new user</param>
        /// <param name="email">Email of the new User</param>
        /// <param name="password">Password of the new User</param>
        /// <returns>returns the new user added to the database, ready to be logged.</returns>
        User CreateUser(string username, string email, string password);

        /// <summary>
        /// Validate a connection upon Credentials: Email and Password
        /// Credentials are based on email because emails can uniquely identify a person.
        /// </summary>
        /// <param name="email">Unique email identifier</param>
        /// <param name="password">Attached password</param>
        /// <returns>return the user if validated, otherwise returns null.</returns>
        User ValidateUser(string email, string password);
    }
}
