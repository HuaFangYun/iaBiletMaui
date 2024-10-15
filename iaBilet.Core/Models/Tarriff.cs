using iaBilet.Core.Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iaBilet.Core.Models
{
    public class Tarriff : INotifyObject
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        public string? displayName { get; set; }
        public decimal? price { get; set; }
        public decimal? listPrice { get; set; }
        public string? currency { get; set; }

        public int? minNrTickets { get; set; }
        public int? maxNrTickets { get; set; }
        public int? eventId { get; set; }
        public List<string>? areas { get; set; }
        public string? notice { get; set; }
        public string? fiscalCodePrefix { get; set; }

        private int _quantity = 0;
        [JsonIgnore]
        [NotMapped]
        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
        }
        [JsonIgnore]
        [NotMapped]
        public string CurrencyName
        {
            get
            {
                if (currency == "RON")
                {
                    return "Lei";
                }
                else
                {
                    return currency;
                }
            }
        }
        
    }
}
