using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iaBilet.Core.Lib
{
    public class ViewModel : INotifyObject
    {
        bool isBusy = false;
        public virtual bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        bool isLoaded = false;
        public bool IsLoaded
        {
            get { return isLoaded; }
            set { SetProperty(ref isLoaded, value); }
        }

        bool requireRefresh = false;
        public bool RequireRefresh
        {
            get { return requireRefresh; }
            set { SetProperty(ref requireRefresh, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public double MainDisplayWidth
        {
            get => Microsoft.Maui.Devices.DeviceDisplay.MainDisplayInfo.Width;
        }

        public double MainDisplayHeight
        {
            get => Microsoft.Maui.Devices.DeviceDisplay.MainDisplayInfo.Height;
        }

        public bool IsPortrait
        {
            get => MainDisplayHeight > MainDisplayWidth;
        }

        public bool IsLandscape
        {
            get => MainDisplayWidth > MainDisplayHeight;
        }

        private ContentPage _contentPage;
        public virtual ContentPage ContentPage
        {
            get => _contentPage;
            set => SetProperty(ref _contentPage, value);
        }

        private Color _color;
        public Color Color
        {
            get => _color ?? Colors.Black;
            set => SetProperty(ref _color, value);
        }

        public void SetTheme()
        {
            if (Application.Current.Resources.TryGetValue("Black", out var colorvalue))
            {
                Color = (Color)colorvalue;
            }
        }

        public void SetSuccessTheme()
        {
            if (Application.Current.Resources.TryGetValue("Succes", out var colorvalue))
            {
                Color = (Color)colorvalue;
            }
        }
        public void SetErrorTheme()
        {
            if (Application.Current.Resources.TryGetValue("Destructive", out var colorvalue))
            {
                Color = (Color)colorvalue;
            }
        }

        public void DisplayAlert(string message, string title = "Info", string button = "close")
        {

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (ContentPage != null)
                {
                    await ContentPage.DisplayAlert(title, message, button);

                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(title, message, button);

                }
            });
        }

        public Task<object?> ShowPopup(Popup popup)
        {
            if (ContentPage == null)
            {
                return Application.Current?.MainPage.ShowPopupAsync(popup);
            }
            else
            {
                return ContentPage.ShowPopupAsync(popup);
            }
        }

    }
}
