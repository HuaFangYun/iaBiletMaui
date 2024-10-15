using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace iaBilet.Core.Services
{
    public class RestServiceResponse
    {
        [JsonProperty("error")]
        public string? Error { get; set; }
        [JsonProperty("errorCode")]
        public string? ErrorCode { get; set; }
        [JsonProperty("errorMessage")]
        public string? ErrorMessage { get; set; }

        [JsonProperty("message")]
        private string message { set { ErrorMessage = value; } }

        [JsonProperty("code")]
        private string code { set { ErrorCode = value; } }

        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonIgnore]
        public string? ResponseString { get; set; }

        [JsonIgnore]
        public JToken? Data {
            get
            {
                if (string.IsNullOrEmpty(ResponseString))
                {
                    return null;
                }
                try
                {
                    return JsonConvert.DeserializeObject<JToken>(ResponseString);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        public bool IsError
        {
            get
            {
                if (!IsSuccessStatusCode)
                {
                    return true;
                }
                return Error != null && Error.ToLower() != "false";

            }
        }

        public bool IsSuccess
        {
            get
            {
                if (!IsSuccessStatusCode)
                {
                    return false;
                }
                return string.IsNullOrEmpty(Error) || Error.ToLower() == "false";
            }
        }

        [JsonIgnore]
        public bool IsSuccessStatusCode { get; set; }
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }
    }
}
