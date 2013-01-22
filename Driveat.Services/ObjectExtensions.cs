using Driveat.Data;
using Driveat.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Driveat.Services
{
    public static class ObjectExtensions
    {
        public static string Value(this Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].StringValue : null;
        }

        public static string ShortMe(this string text, int charnumber)
        {
            if (text.Count() > charnumber)
            {
                text = text.Substring(0, charnumber) + "...";
            }

            return text;
        }
    }
}