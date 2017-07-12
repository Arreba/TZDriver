using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TZDriver.Models.DataModels
{
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public double? Speed { get; set; }
        public double Accuracy { get; set; }
    }
}
