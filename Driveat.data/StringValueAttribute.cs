using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Driveat.Data
{
    /// <summary>
    /// Attribute attached to Reservation state, in order to get the value of each enum value.
    /// </summary>
    public class StringValueAttribute : Attribute
    {
        public string StringValue { get; protected set; }

        public StringValueAttribute(string value)
        {
            this.StringValue = value;
        }
    }
}