using Driveat.Services.UserService;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Driveat.Web.Models
{
    public class UserContext
    {
        private UserContext()
        {
        }

        public static UserContext Current
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Session == null || string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                    return null;

                if ((UserContext)HttpContext.Current.Session["UserContext"] == null)
                {
                    BuildUserContext();
                }

                return (UserContext)HttpContext.Current.Session["UserContext"];
            }
        }

        private static void BuildUserContext()
        {
            BuildUserContext(HttpContext.Current.User);
        }

        private static void BuildUserContext(IPrincipal user)
        {
            if (!user.Identity.IsAuthenticated) return;

            var userService = DependencyResolver.Current.GetService<IUserService>();
            var targetuser = userService.GetUserByEmail(user.Identity.Name);

            if (user == null) return;

            var uc = new UserContext { IsAuthenticated = true };

            var current = new CurrentUser();
            current.id = targetuser._id.ToString();
            current.Username = targetuser.Username;
            current.Email = targetuser.Email;

            uc.CurrentUser = current;

            // Finally, save it into your session
            HttpContext.Current.Session["UserContext"] = uc;
        }


        #region Class members
        public bool IsAuthenticated { get; internal set; }
        public CurrentUser CurrentUser { get; internal set; }
        public string Notify { get; set; }
        public string NotifyType { get; set; }


        // I have these for some user-switching operations I support
        public void Refresh()
        {
            BuildUserContext();
        }

        public void Flush()
        {
            HttpContext.Current.Session["UserContext"] = null;
        }
        #endregion
    }
}