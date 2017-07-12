using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace TZDriver.Models.Tools.Helpers
{
    /// <summary>
    ///     Simple helper for converting a string value to
    ///     its corresponding Enum literal.
    ///     e.g. "Running" -> BackgroundTaskState.Running
    /// </summary>
    public static class EnumHelper
    {
        public static T Parse<T>(string value, bool ignoreCase = true) where T : struct
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

        public static string DeParse<T>(T value) where T : struct
        {
            return Enum.GetName(typeof(T), value);
        }

        public static List<T> EnumToList<T>()
        {
            Type enumType = typeof(T);

            List<T> enumValList = Enum.GetValues(enumType).Cast<T>().ToList();

            return enumValList;
        }

        /// <summary>
        /// Used to get specified Custom Attribute of specified type.
        /// </summary>
        /// <typeparam name="T">Enum Type</typeparam>
        /// <typeparam name="U">Custom attribute Type used in "T"</typeparam>
        public static string GetEnumDescription<T>(string value)
        {
            Type type = typeof(T);
            //Check if "value" is in the Enum
            var name = Enum.GetNames(type).Where(f => f.Equals(value, StringComparison.CurrentCultureIgnoreCase)).Select(d => d).FirstOrDefault();
            if (name == null)
                return string.Empty;

            var field = type.GetTypeInfo().GetDeclaredField(name);
            var attrib = field.GetCustomAttribute(typeof(DisplayAttribute), false) as DisplayAttribute;

            return attrib != null ? attrib.Name : name;
        }

        public static List<string> EnumDescriptionToList<T>()
        {
            Type type = typeof(T);
            List<string> enumValList = new List<string>();
            //Check if "value" is in the Enum
            var enumNames = Enum.GetNames(type);

            foreach(var descrip in enumNames)
            {
                var field = type.GetTypeInfo().GetDeclaredField(descrip);
                var attrib = field.GetCustomAttribute(typeof(DisplayAttribute), false) as DisplayAttribute;

                if(attrib != null)
                {
                    enumValList.Add(attrib.Name.ToUpper());
                }
                else
                {
                    enumValList.Add(descrip.ToUpper());
                }
            }

            return enumValList;
        }
    }

    public class EnumDisplayAttribute : Attribute
    {
        public string Name { get; private set; }

        public EnumDisplayAttribute(string name)
        {
            Name = name;
        }
    }
}
