using Driveat.data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Driveat.Web.Models.Dish
{
    public class EditDishVM
    {

        public string id { get; set; }

        [Required(ErrorMessage="Dish name required")]
        [Display(Name = "Dish name:")]
        public string Name { get; set; }

        [Display(Name = "Dish price:")]
        [Required(ErrorMessage = "Price is required")]
        [Range(2.00, 10.00,ErrorMessage = "Price must be between 2.00 and 10.00")]
        public double Price { get; set; }

        public DateTime Availability { get; set; }

        [Required(ErrorMessage = "Dish food is required")]
        [Display(Name = "Dish food:")]
        public string Food { get; set; }

        [Required(ErrorMessage = "Dish type is required")]
        [Display(Name = "Dish type:")]
        public string SelectedDishType { get; set; }


        public IEnumerable<SelectListItem> DishList
        {
            get
            {
                return DishTypes.Select(dt =>
                new SelectListItem()
                {
                    Text = dt.Name,
                    Value = dt._id.ToString()
                })
                .ToList();
            }
        }


        public List<DishType> DishTypes { get; set; }


        [Required(ErrorMessage = "Dish description is required")]
        [Display(Name = "Dish description:")]
        public string Description { get; set; }

        [Display(Name = "Dish picture:")]
        public HttpPostedFileBase Picture { get; set; }
    }
}