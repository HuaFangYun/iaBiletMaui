using CommunityToolkit.Mvvm.ComponentModel;
using iaBilet.Core.Lib;
using iaBilet.Core.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iaBilet.Core.Services
{
    public class iaBiletRestService : RestService
    {
        private string _key = string.Empty;
        public string Key { get => _key; set => SetProperty(ref _key, value); }

        private string _secret = string.Empty;
        public string Secret { get => _secret; set => SetProperty(ref _secret, value); }

        private string _apiBaseUrl = string.Empty;
        public string ApiBaseUrl { get => _apiBaseUrl; set => SetProperty(ref _apiBaseUrl, value); }

        private string _appId = "1";
        public string AppId { get => _appId; set => SetProperty(ref _appId, value); }
        private string _siteId = "1";
        public string SiteId { get => _siteId; set => SetProperty(ref _siteId, value); }

        private string _authToken = string.Empty;
        public string AuthToken { get => _authToken; set => SetProperty(ref _authToken, value); }
        public string? UrlLogin { get => GetUrl("login"); }
        public string? UrlEvents { get => GetUrl("events"); }
        public string? UrlTarrifs { get => GetUrl("eventTariffs"); }
        public string? UrlLockSeats { get => GetUrl("lockSeats"); }
        public string? UrlPrepareOrder { get => GetUrl("prepareOrder"); }
        public string? UrlConfirmOrder { get => GetUrl("confirmOrder"); }

        public iaBiletRestService() : base()
        {
            string versionInfo = string.Format("{0}/{1}", VersionTracking.CurrentVersion, VersionTracking.CurrentBuild);
            Client.DefaultRequestHeaders.Add("X-APP-VERSION", versionInfo);
            string deviceInfo = string.Format("Models={0};Manufacturer={1};Name={2};VersionString={3};Platform={4};Idiom={5};DeviceType={5}", DeviceInfo.Model, DeviceInfo.Manufacturer, DeviceInfo.Name, DeviceInfo.VersionString, DeviceInfo.Platform, DeviceInfo.Idiom, DeviceInfo.DeviceType);
            Client.DefaultRequestHeaders.Add("X-DEVICE-INFO", deviceInfo);
        }

        public Task<List<Event>> FetchEvents()
        {
            return Task.Run(async () =>
            {
                List<Event> Events = new List<Event>();
                RestServiceResponse response = await FetchPost(UrlEvents);
                if (response.IsSuccess)
                {
                    Log.WriteLine("events =" + response.ResponseString);
                    JArray a = response.Data["events"] as JArray;
                    Events = a.ToObject<List<Event>>();
                    Log.WriteLine("eveons count " + Events.Count() + "ttl" + Events[0].imageUrl);
                }
                return Events;
            });
        }
        public Task<List<Tarriff>> FetchTarrifs(Event Event)
        {
            return Task.Run(async () =>
            {
                List<Tarriff> Tarriffs = new List<Tarriff>();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "eventId" , Event.ID.ToString() } };

                RestServiceResponse response = await FetchPost(UrlTarrifs, parameters);
                if (response.IsSuccess)
                {
                    Log.WriteLine("tariffs =" + response.ResponseString);
                    JArray a = response.Data["tariffs"] as JArray;
                    Tarriffs = a.ToObject<List<Tarriff>>();
                    Log.WriteLine("tarrifs count " + Tarriffs.Count());
                }
                return Tarriffs;
            });
        }

        public Task<RestServiceResponse> LockSeats(Event @event, List<Tarriff> tarrifs)
        {
            return Task.Run(async () =>
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "eventId", @event.ID.ToString() } };
                List<Dictionary<string, object>> seats = new List<Dictionary<string, object>>();
                foreach (Tarriff tarrif in tarrifs)
                {
                    for (int i = 0; i< tarrif.Quantity; i++)
                    {
                        seats.Add(new Dictionary<string, object> {
                            { "tariffId", tarrif.ID.ToString() },
                            { "seatAreaCode", tarrif.areas[0] },
                            { "ref", Guid.NewGuid().ToString() },
                            { "price", tarrif.price.GetValueOrDefault() }
                        });
                    }
                }

                parameters["seats"] = seats;
                RestServiceResponse response = await FetchPost(UrlLockSeats, parameters);
                return response;
            });
        }

        public Task<RestServiceResponse> PrepareOrder(Booking Booking)
        {
            return Task.Run(async () =>
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { 
                    { "bookingRef", Booking.bookingRef },
                    { "totalPrice", Booking.Total.totalPrice },
                    {"customerInfo",  new Dictionary<string, string>() { { "email", "gigiduru@mailinator.com" }, { "phone", "0732410779" } } }
                };
                RestServiceResponse response = await FetchPost(UrlPrepareOrder, parameters);
                return response;
            });
        }



        public override Task<RestServiceResponse> FetchPost(string absoluteUrl, Dictionary<string, object> parameters = null, Dictionary<string, object> urlParams = null)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, object>();
            }
            if (! string.IsNullOrEmpty(AuthToken))
            {
                parameters["authToken"] = AuthToken;
            }
            Uri uri = GetUri(absoluteUrl, urlParams);
            return Fetch(uri, parameters);
        }
        private string GetUrl(string path)
        {
            string url = string.Format("{0}/{1}", ApiBaseUrl.TrimEnd('/'), path);
            return url;
        }
    }
}
