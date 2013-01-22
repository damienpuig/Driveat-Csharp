using Driveat.Services.DishService;
using Driveat.Services.UserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Driveat.Services;
using Ninject;

namespace Driveat.Web.Controllers
{
    public class UserController : BaseController
    {
        public IUserService UserSvc { get; set; }
        public IDishService DishSvc { get; set; }

        //NInject attribute to inject services in the contructor. Controllers are independent from service layer.
        [Inject]
        public UserController(IUserService userservice, IDishService dishservice)
        {
            UserSvc = userservice;
            DishSvc = dishservice;
        }

        //G
        // GET: /User/
        public ActionResult Show(string username)
        {
            //If username is null, redirect.
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Index", "Home");
            }
            var user = UserSvc.GetUserByUsername(username);

            //If user is null, redirect.
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Get the dishes.
            user.Dishes = DishSvc.GetDishesByUserId(user._id.ToString());

            if (user.Dishes != null)
                user.Dishes.ForEach(dish =>
                    {
                        dish.Food = dish.Food.ShortMe(100);
                        dish.Description = dish.Description.ShortMe(100);
                    });

            return View(user);
        }
    }
}
