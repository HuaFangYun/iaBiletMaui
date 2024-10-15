using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace iaBilet.Core.Models
{
    public class Event
    {
        [JsonProperty("id")]
        public string? ID { get; set; }
        public string? startDate { get; set; }
        public string? endDate { get; set; }
        public string? startDateTime { get; set; }
        public bool? hasTime { get; set; }
        public string? venue { get; set; }
        public string? title { get; set; }
        public string? venueId { get; set; }
        public bool? allowBooking { get; set; }

        // daca sunt permise rezervari (iar daca nu, motivul)
        // valori posibile: "Y", "sold_out", "venue_only"

        public string? allowBookingEx { get; set; }
        public int? maxNrTicketsPerOrder { get; set; }
        public bool? soldOut { get; set; }
        public bool? hasAvailableSeats { get; set; }
        public string? imageUrl { get; set; }
        public string? shortDesc { get; set; }
        public string? fullDesc { get; set; }
        public string? eventPageUrl { get; set; }
        public string? category { get; set; }
        public List<string>? moreCategories { get; set; }
        public List<string>? tag { get; set; }
        [JsonProperty("artists")]
        List<EventArtist>? Artists { get; set; }
        [NotMapped]
        [JsonIgnore]
        public string? ShortDescription
        {
            get => WebUtility.HtmlEncode(shortDesc);
        }

        [NotMapped]
        [JsonIgnore]
        public string? Title
        {
            get => WebUtility.HtmlEncode(title);
        }
        [NotMapped]
        [JsonIgnore]
        public string? DisplayDate
        {
            get =>string.Format("{0} {1}", startDate, endDate);
        }


    }
}
