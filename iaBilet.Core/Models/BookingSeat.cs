using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iaBilet.Core.Models
{
    public class BookingSeat
    {
        [JsonProperty("uuid")]
        public string? UID { get; set; }
        public string? status { get; set; }
        public int? tariffId { get; set; }
        public bool? hasSeat { get; set; }
        public string? seatCode { get; set; }
        public string? areaCode { get; set; } = string.Empty;
        [JsonProperty("ref")]
        public string? reference { get; set; }
    }
}
