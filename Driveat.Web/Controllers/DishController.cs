using Driveat.data;
using Driveat.Services.DishService;
using Driveat.Services.EnumServices;
using Driveat.Services.UserService;
using Driveat.Web.Models;
using Driveat.Web.Models.Dish;
using MongoDB.Bson;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Driveat.Services.ImageService;
using System.Configuration;
using Driveat.Services;

namespace Driveat.Web.Controllers
{
    /// <summary>
    /// Authorize attribute needed on the Account controller, managing accounts.
    /// </summary>
    [Authorize]
    public class DishController : BaseController
    {
        public IDishService DishSvc { get; set; }
        public IUserService UserSvc { get; set; }
        public IDishTypeService DishTypeSvc { get; set; }
        public IImageService ImageSvc { get; set; }

        //NInject attribute to inject services in the contructor. Controllers are independent from service layer.
        [Inject]
        public DishController(IDishService dishservice, IUserService userservice, IDishTypeService dishtypeservice, IImageService imageservice)
        {
            DishSvc = dishservice;
            UserSvc = userservice;
            DishTypeSvc = dishtypeservice;
            ImageSvc = imageservice;
        }

        //A dish is accessible by it id and display on the /Dish/Show page
        // GET: /Dish/
        public ActionResult Show(string dishid)
        {
            var dish = DishSvc.GetDishById(dishid);

            //If notifications has been affected, display it and reset to null.
            ViewBag.Notify = UserContext.Current.Notify;
            ViewBag.NotifyType = UserContext.Current.NotifyType;
            UserContext.Current.Notify = null;
            UserContext.Current.NotifyType = null;

            return View(dish);
        }

        //
        // GET: /Dish/New
        public ActionResult New()
        {
            // DishVM and dish types  initialisations.
            var dishtoAdd = new DishVM();
            dishtoAdd.DishTypes = DishTypeSvc.GetDishTypes();

            return View(dishtoAdd);
        }

        //
        // POST: /Dish/New
        [HttpPost]
        public ActionResult New(DishVM newDish)
        {
            //Check if the model is valid ( newDish depending on property validations
            if (ModelState.IsValid)
            {
                //Get the current user
                var currentUser = UserContext.Current.CurrentUser;
                var user = UserSvc.GetUserById(currentUser.id);

                var dishType = DishTypeSvc.GetDishTypeById(newDish.SelectedDishType);

                //Create a new dish object instance.
                var dish = new Dish()
                {
                    Availability = DateTime.Now.AddMonths(1),
                    Description = newDish.Description,
                    Dishtype = dishType,
                    Food = newDish.Food,
                    Name = newDish.Name,
                    Price = newDish.Price,
                    Seller = new NestedUser()
                    {
                        _id = user._id,
                        Email = user.Email,
                        Username = user.Username
                    }
                };


                //Add the creation to the database
              var createdDish =  DishSvc.CreateADish(dish);

                //Check for picture file
              var filename = ImageSvc.SaveImage(newDish.Picture, createdDish._id.ToString(), Server.MapPath(ConfigurationManager.AppSettings["dishdirpicture"]));
               createdDish.Picture = filename;

                //Update the above dish with generated picture filename.
              DishSvc.UpdateDish(createdDish);


                // notification.
                UserContext.Current.Notify = " Dish Added !";
                UserContext.Current.NotifyType = notificationType.success.Value();

                return RedirectToAction("Index", "Account");

            }
            
            
            return View(newDish);
        }

        //
        // GET: /Dish/Edit/id
        public ActionResult Edit(string id)
        {
            //Get the dish to be edited.
            var dishToEdit = DishSvc.GetDishById(id);

            //generate ( wrong : the editDishViewmodel has to be mapped from a service layer dto..) the edit dish object from entity object.
            var model = new EditDishVM()
            {
                id= dishToEdit._id.ToString(),
                Availability = dishToEdit.Availability,
                Description = dishToEdit.Description,
                Food = dishToEdit.Food,
                SelectedDishType = dishToEdit.Dishtype._id.ToString(),
                Name = dishToEdit.Name,
                Price = dishToEdit.Price,
               
                
            };

            // Dish types  initialisations.
            model.DishTypes = DishTypeSvc.GetDishTypes();
            return View(model);
        }

        //Update pretty similar than a new, but the dish has been previously initialized and setted again.
        // POST: /Dish/Edit/id
        [HttpPost]
        public ActionResult Edit(EditDishVM editDishViewModel)
        {
            try
            {
                var currentUser = UserContext.Current.CurrentUser;
                var currentDish = DishSvc.GetDishById(editDishViewModel.id);
                 var dishType = DishTypeSvc.GetDishTypeById(editDishViewModel.SelectedDishType);
                var user = UserSvc.GetUserById(currentUser.id);

                var editedDish = new Dish()
                {
                    _id = currentDish._id,
                    Availability = editDishViewModel.Availability,
                    Description = editDishViewModel.Description,
                    Dishtype = dishType,
                    Food = editDishViewModel.Food,
                    Name = editDishViewModel.Name,
                    Price = editDishViewModel.Price,
                    Seller = new NestedUser()
                    {
                        _id = user._id,
                        Email = user.Email,
                        Username = user.Username
                    },
                    Picture = currentDish.Picture
                };

                var filename = ImageSvc.UpdateImage(editDishViewModel.Picture, currentDish._id.ToString(), Server.MapPath(ConfigurationManager.AppSettings["dishdirpicture"]), currentDish.Picture);
                
                if(filename != null)
                    editedDish.Picture = filename;

                DishSvc.UpdateDish(editedDish);

                // TODO: Add update logic here

                return RedirectToAction("Index", "Account");
            }
            catch
            {
                return RedirectToAction("Index", "Account");
            }
        }

        //
        // GET: /Dish/Delete/5

        public ActionResult Delete(string id)
        {
            //Delete a dish by giving an id
            DishSvc.DeleteDish(id);
            return RedirectToAction("Index", "Account");
        }
    }
}
