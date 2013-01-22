using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using Driveat.Web.Models;
using Driveat.Services.UserService;
using Ninject;
using System.Security.Principal;
using Driveat.Services.DishService;
using Driveat.data;
using MongoDB.Bson;
using Driveat.Services;
using GeoCoding;
using System.IO;
using Driveat.Services.ImageService;
using System.Configuration;
using Driveat.Services.ReservationService;

namespace Driveat.Web.Controllers
{
    /// <summary>
    /// Authorize attribute needed on the Account controller, managing accounts.
    /// </summary>
    [Authorize]
    public class AccountController : BaseController
    {

        public IUserService UserSvc { get; set; }
        public IDishService DishSvc { get; set; }
        public IGeoCoder GeocodeSvc { get; set; }
        public IImageService ImageSvc { get; set; }
        public IReservationService ReservationSvc { get; set; }

        //NInject attribute to inject services in the contructor. Controllers are independent from service layer.
        [Inject]
        public AccountController(IUserService userservice, IDishService dishservice, IGeoCoder geocodeservice, IImageService imageservice, IReservationService reservationservice)
        {
            UserSvc = userservice;
            DishSvc = dishservice;
            GeocodeSvc = geocodeservice;
            ImageSvc = imageservice;
            ReservationSvc = reservationservice;

        }


        #region LOGIN / LOGOUT

        //
        // GET: /Account/Login
        // Allow anonymous attribute let the user to render the page without taking in account the Authorize controller attribute.
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            //Log the user if the model sent is valid.
            if (ModelState.IsValid)
            {
                var user = UserSvc.ValidateUser(model.Email, model.Password);

                if (user == null)
                {
                    // If we got this far, something failed, redisplay form
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                    return View(model);
                }


                //If the user is validated, authenticate it.
                SetAuthenticationCookie(model.Email);
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // POST: /Account/LogOff
        public ActionResult LogOff()
        {
            //Sign out the current user and flush the context.
            FormsAuthentication.SignOut();
            UserContext.Current.Flush();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            //If the model is valid, attempt to create and validate to user. the user.
            if (ModelState.IsValid)
            {
                
                   var result = UserSvc.CreateUser(model.UserName, model.Email, model.Password);
                   var current = UserSvc.ValidateUser(model.Email, model.Password);

                   if (current != null && result != null)
                   {
                       //Upload the picture, and authenticate.

                       var filename = ImageSvc.SaveImage(model.Picture, result._id.ToString(), Server.MapPath("~/COMMON/user/"));
                       result.Picture = filename;
                       UserSvc.UpdateUser(result);

                        SetAuthenticationCookie(model.Email);
                        return RedirectToAction("Index", "Account");
                    }
            }

            ModelState.AddModelError("", "Something goes wrong and been notified.");
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual void SetAuthenticationCookie(string email)
        {
            //Log without fixed cookie.
            FormsAuthentication.SetAuthCookie(email, false);
        }

        #endregion

        #region MANAGE

        //Huge Account index containing:
        //User information ready to be updated.
        //User dishes ready to be updated.
        //User reservations if any.
        //User sales if any.
        // Account index
        public ActionResult Index()
        {
            var user = UserSvc.GetUserById(UserContext.Current.CurrentUser.id);

            user.Dishes = DishSvc.GetDishesByUserId(UserContext.Current.CurrentUser.id);
            user.Dishes.ForEach(dish =>
                {
                   dish.Food = dish.Food.ShortMe(100);
                   dish.Description = dish.Description.ShortMe(100);
                });

            user.Reservations = ReservationSvc.GetReservationByBuyerId(UserContext.Current.CurrentUser.id);
            user.Sales = ReservationSvc.GetReservationBySellerId(UserContext.Current.CurrentUser.id);

            ViewBag.Notify = UserContext.Current.Notify;
            ViewBag.NotifyType = UserContext.Current.NotifyType;
            UserContext.Current.Notify = null;
            UserContext.Current.NotifyType = null;

            return View(user);
        }

        [HttpPost]
        public ActionResult Update(User updatedUser, HttpPostedFileBase file)
        {
            
            if (ModelState.IsValid)
            {
                var user = UserContext.Current.CurrentUser;
                var currentuser = UserSvc.GetUserById(user.id);

                currentuser.Firstname = updatedUser.Firstname;
                currentuser.Lastname = updatedUser.Lastname;
                currentuser.AboutYou = updatedUser.AboutYou;

                Address result = null;

                if (!string.IsNullOrEmpty(updatedUser.Location.Address))
                {
                    result = GeocodeSvc.GeoCode(updatedUser.Location.Address).FirstOrDefault();
                }

                if (result != null)
                {
                    if (currentuser.Location == null)
                        currentuser.Location = new NestedLocation();

                    currentuser.Location.Address = updatedUser.Location.Address;
                    currentuser.Location.Address = result.FormattedAddress;
                    currentuser.Location.Coordinates = new double[2
                        ];

                    currentuser.Location.Coordinates[0] = result.Coordinates.Longitude;
                    currentuser.Location.Coordinates[1] = result.Coordinates.Latitude;

                    UserContext.Current.Notify = "Profile updated !";
                }
                else
                {
                    UserContext.Current.Notify = "Profile updated with warning: Address not retreived.";
                }

                var upload = ImageSvc.UpdateImage(file, currentuser._id.ToString(), Server.MapPath(ConfigurationManager.AppSettings["userdirpicture"]), currentuser.Picture);

                if (!string.IsNullOrEmpty(upload))
                    currentuser.Picture = upload;

                UserSvc.UpdateUser(currentuser);

                UserContext.Current.NotifyType = notificationType.success.Value();
            }

            return RedirectToAction("Index");
        }






        #endregion

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion
    }
}
