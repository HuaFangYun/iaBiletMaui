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
    public class LoginViewModel : AppViewModel
    {
        public LoginViewModel(iaBiletRestService restService, SettingsViewModel settingsViewModel) : base(restService, settingsViewModel)
        {
        }

        public Task<bool> LoadCredentials()
        {
            return Task.Run(async () =>
            {
                Dictionary<string, object> credentials = new Dictionary<string, object>
                {
                    { "key", SettingsViewModel.Key },
                    { "secret", SettingsViewModel.Secret },
                    { "appId", SettingsViewModel.AppId },
                    { "siteId", SettingsViewModel.SiteId }
                };

                RestServiceResponse response = await RestService.FetchPost(RestService.UrlLogin, credentials);
                if (response.IsSuccess)
                {
                    RestService.AuthToken = (string)response.Data["authToken"];
                    Log.WriteLine("AuthToken =" + RestService.AuthToken);
                }
                return response.IsSuccess;
            });
        }
    }
}
