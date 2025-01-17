﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iaBilet.Core.Models
{
    public class BookingPriceModifier
    {
        [JsonProperty("type")]
        public string? Type { get; set; }
        [JsonProperty("label")]
        public string? Label { get; set; }
        [JsonProperty("value")]
        public string? Value { get; set; }
    }
}
