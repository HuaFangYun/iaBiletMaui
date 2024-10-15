using iaBilet.Core.Lib;
using iaBilet.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iaBilet.ViewModels.Settings
{
    public partial class SettingsViewModel : ViewModel
    {
        private const string BLE_CONECTIVITY_TYPE = "BLUETHOOT";
        private const string SERIAL_CONECTIVITY_TYPE = "SERIAL";
        private const string NETWORK_CONECTIVITY_TYPE = "TCPIP";

        public SettingsViewModel()
        {
            iaBiletRestService restService = Application.Current.Handler.MauiContext.Services.GetService<iaBiletRestService>();
            restService.ApiBaseUrl = ApiBaseUrl;
            restService.Key = Key;
            restService.Secret = Secret;
            restService.AppId = AppId;
            restService.SiteId = SiteId;
            LoadPosSettings();
        }

        private string[] _availablePorts;
        public string[] AvailablePorts
        {
            get => _availablePorts ?? new string[0];
            set => SetProperty(ref _availablePorts, value);
        }
    }
}
