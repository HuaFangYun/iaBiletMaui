using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iaBilet.Core.Models
{
    public class Order
    {
        public string? orderNr { get; set; }
        public string? printUrl { get; set; }
        [JsonProperty("totals")]
        public BookingTotal? Total { get; set; }
        [JsonProperty("tickets")]
        public List<Ticket>? Tickets { get; set; }

    }
}
