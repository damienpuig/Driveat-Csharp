using Driveat.data;
using Driveat.Services.EnumServices;
using Driveat.Services.UserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Driveat.Web.Models;
using Ninject;
using Driveat.Services.DishService;

namespace Driveat.Web.Controllers
{
    public class HomeController : BaseController
    {
       
        public  IDishTypeService DishTypeSvc { get; set; }

        public  IUserService UserSvc { get; set; }

        public  IDishService DishSvc { get; set; }

        //NInject attribute to inject services in the contructor. Controllers are independent from service layer.
         [Inject]
        public HomeController(IDishTypeService dishtypeservice, IUserService userservice, IDishService dishservice)
        {
            DishTypeSvc = dishtypeservice;
            UserSvc = userservice;
            DishSvc = dishservice;
        }

        //Get home page, doesn't affect a SearchViewModel because it is not needed in that case (no Model encapsulations.)
        public ActionResult Index()
        {
            return View();
        }
    }
}
