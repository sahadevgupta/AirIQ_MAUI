using System;
using System.Collections.Generic;
using System.Text;

namespace AirIQ.Models
{
    public class Infant : Passenger
    {
        public int AssignedAdultId { get; set; }
    }
}
