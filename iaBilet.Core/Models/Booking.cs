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
    public class Booking : INotifyObject
    {
        public int? eventId { get; set; }
        public string? eventTitle { get; set; }
        public string? status { get; set; }
        public string? bookingRef { get; set; }
        [JsonProperty("seats")]
        public List<BookingSeat>? Seats { get; set; }
        [JsonProperty("totals")]
        public BookingTotal? Total { get; set; }
        private List<Tarriff> _tarriffs = new List<Tarriff>();
        [NotMapped]
        [JsonIgnore]
        public List<Tarriff> Tarriffs { get => _tarriffs; set => SetProperty(ref _tarriffs, value); }
    }
}
