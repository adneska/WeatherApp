using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Model
{
    public class WeatherData
    {
        public double Temperature { get; set; }
        public string Details { get; set; }
        public DateTime Sunrise { get; set; }
        public DateTime Sunset { get; set; }
        public double WindSpeed { get; set; }
        public double Pressure { get; set; }
    }
}
