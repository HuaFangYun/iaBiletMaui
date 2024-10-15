using iaBilet.Core.Lib;
using iaBilet.Core.Models;
using iaBilet.Core.Services;
using iaBilet.ViewModels.Settings;
using iaBilet.ViewModels;
using iaBilet.Views.Pages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using iaBilet.Settings;

namespace iaBilet
{
    public class MainPageViewModel : AppViewModel
    {
        public MainPageViewModel(iaBiletRestService restService, SettingsViewModel settingsViewModel) : base(restService, settingsViewModel)
        {
            InitCommands();
        }
        public LoginViewModel LoginViewModel { get => Application.Current.Handler.MauiContext.Services.GetService<LoginViewModel>(); }
        public EventsViewModel EventsViewModel { get => Application.Current.Handler.MauiContext.Services.GetService<EventsViewModel>(); }
        private CartViewModel? _cartViewModel = null;
        public CartViewModel CartViewModel
        {
            get => _cartViewModel;
            set => SetProperty(ref _cartViewModel, value);
        }


        public async Task<bool> LoadData()
        {
            if (CartViewModel == null)
            {
                CartViewModel = Application.Current.Handler.MauiContext.Services.GetService<CartViewModel>();
                CartViewModel.BookingCompleted += async (s, e) =>
                {
                    FinalizePopup finalizePage = new FinalizePopup();
                    try
                    {
                        await ShowPopup(finalizePage);
                    }
                    catch (Exception ex) { }
                };
            }

            bool result = await LoginViewModel.LoadCredentials();
            if (result)
            {
                result = await EventsViewModel.LoadEvents();
            }
            return result;
        }

        public ICommand EventClickCommand { private set; get; }
        public ICommand CloseEventViewCommand { private set; get; }
        public ICommand CloseFinalizeViewCommand { private set; get; }
        public ICommand OpenSettingsCommand { private set; get; }
        
        private void InitCommands()
        {
            EventClickCommand = new Command<Event>(execute: (e) => {
                ExecuteEventClickCommand(e);
            }, canExecute: (e) => true);
            CloseEventViewCommand = new Command(execute: () => {
                ExecuteCloseEventViewCommand();
            }, canExecute: () => !IsBusy || !CartViewModel.IsBusy);
            CloseFinalizeViewCommand = new Command(execute: () => {
                ExecuteCloseFinalizeViewCommand();
            }, canExecute: () => !IsBusy || !CartViewModel.IsBusy);
            OpenSettingsCommand = new Command(execute: () => {
                ExecuteOpenSettingsCommand();
            }, canExecute: () => !IsBusy || !CartViewModel.IsBusy);
        }

        public async void ExecuteEventClickCommand(Event @event)
        {
            Log.WriteLine("clikc on" + JsonConvert.SerializeObject(@event));
            CartViewModel.Event = @event;

            EventPage eventPage = new EventPage();
            MainThread.BeginInvokeOnMainThread(async () => {
                try
                {
                    //await App.Current.MainPage.Navigation.PushAsync(eventPage);
                    EventPopup eventPopup = new EventPopup();
                    await ShowPopup(eventPopup);
                    FinalizePopup finalizePopup = new FinalizePopup();
                    await ShowPopup(finalizePopup);
                }
                catch (Exception ex) { }
            });

            RefreshCanExecutes();
        }

        public async void ExecuteCloseEventViewCommand()
        {
            CartViewModel.Reset();
            MainThread.BeginInvokeOnMainThread(async () => {
                try
                {
                    await App.Current.MainPage.Navigation.PopModalAsync();
                }
                catch (Exception ex) { }
            });

            RefreshCanExecutes();
        }
        public async void ExecuteCloseFinalizeViewCommand()
        {
            MainThread.BeginInvokeOnMainThread(async () => {
                try
                {
                    await App.Current.MainPage.Navigation.PopModalAsync();
                }
                catch (Exception ex) { }
            });

            RefreshCanExecutes();
        }

        public async void ExecuteOpenSettingsCommand()
        {
            AdminPage settingsPage = new AdminPage();
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await App.Current.MainPage.Navigation.PushAsync(settingsPage);
                }
                catch (Exception ex) { }
            });

            RefreshCanExecutes();
        }

        public void RefreshCanExecutes()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                (EventClickCommand as Command).ChangeCanExecute();
                (CloseEventViewCommand as Command).ChangeCanExecute();
                (CloseFinalizeViewCommand as Command).ChangeCanExecute();
            });
        }
    }
}
