using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace Driveat.Web.Models
{
    public class CurrentUser
    {
        public string id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

    }
}