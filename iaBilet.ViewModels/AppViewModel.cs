using iaBilet.Core.Lib;
using iaBilet.Core.Services;
using iaBilet.ViewModels.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iaBilet.ViewModels
{
    public class AppViewModel : ViewModel
    {
        private iaBiletRestService? _restService;
        private SettingsViewModel? _settingsViewModel;

        public virtual iaBiletRestService RestService
        {
            get => _restService ?? throw new InvalidOperationException("RestService is not initialized.");
            set => _restService = value;
        }

        public virtual SettingsViewModel SettingsViewModel
        {
            get => _settingsViewModel ?? throw new InvalidOperationException("SettingsViewModel is not initialized.");
            set => _settingsViewModel = value;
        }

        public AppViewModel(iaBiletRestService restService, SettingsViewModel settingsViewModel)
        {
            RestService = restService;
            SettingsViewModel = settingsViewModel;
        }
       
    }
}
