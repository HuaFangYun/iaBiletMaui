using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iaBilet.ViewModels.Settings
{
    public partial class SettingsViewModel
    {
        public string _apiBaseUrl = string.Empty;
        public string ApiBaseUrl
        {
            get { 
                string value =  string.IsNullOrEmpty(_apiBaseUrl) ? Preferences.Get("ApiBaseUrl", "") : _apiBaseUrl; 
                return string.IsNullOrEmpty(value) ? "https://sandbox01.iabilet.dev/api/1.2" : value;
            }
            set { SetProperty(ref _apiBaseUrl, value); Preferences.Set("ApiBaseUrl", _apiBaseUrl); }
        }

        public string _key =  string.Empty;
        public string Key
        {
            get {
              
                string value =  string.IsNullOrEmpty(_key) ? Preferences.Get("Key", "") : _key;
                return string.IsNullOrEmpty(value) ? "4jcRRm3JDGkwb1aK" : value;
            }
            set { SetProperty(ref _key, value); Preferences.Set("Key", _key); }
        }

        public string _secret = string.Empty;
        public string Secret
        {
            get
            {
                string value = string.IsNullOrEmpty(_secret) ? Preferences.Get("Secret", "") : _secret;
                return string.IsNullOrEmpty(value) ? "LPXvG6WKZQBehIwl" : value;
            }
            set { SetProperty(ref _secret, value); Preferences.Set("Secret", _secret); }
        }



        private string _appId = "1";
        public string AppId { get => _appId; set => SetProperty(ref _appId, value); }
        private string _siteId = "1";
        public string SiteId { get => _siteId; set => SetProperty(ref _siteId, value); }


    }
}
