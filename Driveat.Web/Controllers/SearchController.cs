using Driveat.data;
using Driveat.Services.DishService;
using Driveat.Services.SearchService;
using Driveat.Services.UserService;
using GeoCoding;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Driveat.Services;

namespace Driveat.Web.Controllers
{
    public class SearchController : BaseController
    {
        public IDishService DishSvc { get; set; }
        public ISearchService SearchSvc { get; set; }

        //NInject attribute to inject services in the contructor. Controllers are independent from service layer.
        [Inject]
        public SearchController(IDishService dishservice, ISearchService searchservice)
        {
            DishSvc = dishservice;
            SearchSvc = searchservice;
        }

        //Search a dish by giving an address and a search scope.
        // GET: /Search/
        public ActionResult List(string search, int distance)
        {
            //If get is null, send Home.
            if (!string.IsNullOrEmpty(search))
            {
                //Search by location and scope ( Described in the service. )
                var results = SearchSvc.SearchDishesByLocationAndScope(search, distance).ToList();

                // If the result contains elements, format string to 100 characters long.
                if (results.Count > 0)
                    results.ForEach(dish =>
                        {
                            dish.Food = dish.Food.ShortMe(100);
                            dish.Description = dish.Description.ShortMe(100);
                        });

                //Send result even if it is null.
                return View(results);
            }

            return RedirectToAction("Index", "Home");
        }

    }
}
