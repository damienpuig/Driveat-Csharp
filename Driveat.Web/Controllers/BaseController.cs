using Driveat.Data;
using Driveat.Services;
using Driveat.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Driveat.Web.Controllers
{
    /// <summary>
    /// The base controller in an abstraction of the Controller object provided by Mvc framework. 
    /// It Attempts to share notification informations.
    /// </summary>
    public class BaseController : Controller
    {
        public enum notificationType
        {
            [StringValue("error")]
            error,
            [StringValue("success")]
            success
        }
    }
}
