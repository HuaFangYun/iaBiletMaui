using iaBilet.Core.Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iaBilet.Core.Models
{
    public class BookingTotal : INotifyObject
    {
        public decimal? totalBeforeModifiers { get; set; }
        public decimal? totalPrice { get; set; }
        public string? totalPriceCurrenty { get; set; }
        public decimal? totalPaid { get; set; }
        public string? totalPaidCurrency { get; set; }
        [JsonProperty("priceModifiers")]
        public List<BookingPriceModifier>? PriceModifiers { set; get; }
    }
}
