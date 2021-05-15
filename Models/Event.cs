using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TesteEbanx.Models
{
    public class Event
    {
        public string Type { get; set; }
        public string Destination { get; set; }
        public int Amount { get; set; }
        public string Origin { get; set; }

    }
}
