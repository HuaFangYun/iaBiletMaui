using iaBilet.Core.Lib;
using iaBilet.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iaBilet.Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using iaBilet.ViewModels.Settings;

namespace iaBilet.ViewModels
{
    public class EventsViewModel : AppViewModel
    {
        public EventsViewModel(iaBiletRestService restService, SettingsViewModel settingsViewModel) : base(restService, settingsViewModel)
        {
        }
        protected List<Event> _events;
        public List<Event> Events
        {
            get => _events;
            set => SetProperty(ref _events, value);
        }

        protected Event _event;
        public Event Event
        {
            get => _event;
            set => SetProperty(ref _event, value);
        }

        public Task<bool> LoadEvents()
        {
            return Task.Run(async () =>
            {
                RestServiceResponse response = await RestService.FetchPost(RestService.UrlEvents);
                if (response.IsSuccess)
                {
                    Log.WriteLine("events =" + response.ResponseString);
                    JArray a  = response.Data["events"] as JArray;
                    Events = a.ToObject<List<Event>>();
                    Log.WriteLine("events count " + Events.Count() + "ttl" + Events[0].imageUrl);
                }
                return response.IsSuccess;
            });
        }
    }
}
