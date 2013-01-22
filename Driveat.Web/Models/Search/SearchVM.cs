using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Driveat.Web.Models.Search
{
    public class SearchVM
    {
        [Required]
        [StringLength(10, ErrorMessage="Your address must be longer...")]
        public string search { get; set; }
    }
}