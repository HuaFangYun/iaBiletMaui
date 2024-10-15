using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iaBilet.Core.Models
{
    public class Ticket
    {
        public string? code { get; set; }
        public string? fiscalCode { get; set; }
        public string? shortCode { get; set; }
        public string? eventTitle { get; set; }
        public string? venueName { get; set; }
        public string? tariffName { get; set; }
        public decimal? price { get; set; }
        public decimal? currency { get; set; }

    }
}
