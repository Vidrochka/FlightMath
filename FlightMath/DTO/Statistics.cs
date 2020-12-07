using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMath.DTO
{
    public class Statistics
    {
        public decimal Sequence { get; set; }
        public string Carrier { get; set; }
        public string Flights { get; set; }
        public string Dates { get; set; }
        public string Origin { get; set; }
        public string Dest { get; set; }
        public int AWBcount { get; set; }
        public decimal Weight { get; set; }
        public decimal Distance { get; set; }
        public decimal Revenue { get; set; }
    }
}
