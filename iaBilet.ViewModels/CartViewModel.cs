using iaBilet.Core.Services;
using iaBilet.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using iaBilet.Core.Lib;
using System.Windows.Input;
using Newtonsoft.Json;
using iaBilet.ViewModels.Settings;
using iaBilet.Pos;


namespace iaBilet.ViewModels
{
    public class BookingEventArgs : EventArgs
    {
        public Booking? Booking { get; set; }
    }

    public class CartItem
    {
        public int Quantity { set; get; }
        public decimal Price { set; get; }
        public decimal Total { set; get; }
        public string Name { set; get; }
        public string Currency { set; get; }
        public string CurrencyName { set; get; }
        public Tarriff Tarriff { set; get; }


    }
    public class CartViewModel : AppViewModel
    {
        public event EventHandler<BookingEventArgs> BookingCompleted;
        protected virtual void OnBookingCompleted(BookingEventArgs e)
        {
            BookingCompleted?.Invoke(this, e);
        }

        protected Event _event;
        public Event Event
        {
            get => _event;
            set => SetProperty(ref _event, value);
        }

        private List<BookingPriceModifier> _priceModifiers;
        public List<BookingPriceModifier>? PriceModifiers { get => _priceModifiers; set => SetProperty(ref _priceModifiers, value); }

        protected Booking _booking;
        public Booking Booking
        {
            get => _booking;
            set  {
                SetProperty(ref _booking, value);
                Total = Math.Round(_booking.Total?.totalPrice.GetValueOrDefault() ?? 0,2);
                PriceModifiers = _booking.Total?.PriceModifiers;
                Console.WriteLine("PriceModifiers count" + PriceModifiers.Count);
            }
        }

        private ObservableCollection<Tarriff> _tarriffs;
        public ObservableCollection<Tarriff> Tarriffs
        {
            get => _tarriffs;
            set => SetProperty(ref _tarriffs, value);
        }

        private List<CartItem> _cartItems;
        public List<CartItem> CartItems
        {
            get => _cartItems;
            set => SetProperty(ref _cartItems, value);
        }

        

        private decimal _total = 0;
        public decimal Total
        {
            get => _total;
            set => SetProperty(ref _total, value);
        }
        public CartViewModel(iaBiletRestService restService, SettingsViewModel settingsViewModel) : base(restService, settingsViewModel)
        {
            InitCommands();
        }

        public void Reset()
        {
            Event = null;
            Tarriffs = null;
        }
        public Task<bool> LoadTarrifs()
        {
            return Task.Run(async () =>
            {
                try
                {
                    List<Tarriff> tarrifs = await RestService.FetchTarrifs(Event);
                    Tarriffs = new ObservableCollection<Tarriff>(tarrifs);
                }
                catch (Exception e)
                {
                    Log.WriteLine("Error loading tarrifs: " + e.Message);
                }
                return Tarriffs!= null && Tarriffs.Count > 0;
            });
        }

        public List<Tarriff> SelectedTarrrifs => Tarriffs?.Where(t => t.Quantity > 0).ToList();

        public string EventImageUrl
        {
            get => Event.imageUrl;
        }

        public ICommand IncrementCommand { private set; get; }
        public ICommand DecrementCommand { private set; get; }
        public ICommand LockSeatsCommand { private set; get; }
        public ICommand PayBookingCommand { private set; get; }

        

        private void InitCommands()
        {

            IncrementCommand = new Command<Tarriff>(execute: (tarriff) =>
            {
                ExecuteIncrementCommand(tarriff);
            }, canExecute: (Tarriff) => { return !IsBusy; });

            DecrementCommand = new Command<Tarriff>(execute: (tarriff) =>
            {
                ExecuteDecrementCommand(tarriff);
            }, canExecute: (tarriff) => { return  !IsBusy && (tarriff!= null && tarriff.Quantity > 0); });
            LockSeatsCommand = new Command(execute: () =>
            {
                Log.WriteLine("Executing LockSeatsCommand");
                ExecuteLockSeatsCommand();
            }, canExecute: () => { return !IsBusy && (SelectedTarrrifs != null && SelectedTarrrifs.Count() > 0); });
            PayBookingCommand = new Command(execute: () =>
            {
                Log.WriteLine("Executing PayBookingCommand");
                ExecutePayBookingCommand();
            }, canExecute: () => { return Booking!=null; });
        }
        private async void ExecuteIncrementCommand(Tarriff tarrif)
        {
            tarrif.Quantity++;
            RefreshCanExecutes();
        }
        private async void ExecuteDecrementCommand(Tarriff tarrif)
        {
            if (tarrif.Quantity == 0)
            {
                return;
            }
            tarrif.Quantity--;
            RefreshCanExecutes();
        }

        private async void ExecuteLockSeatsCommand()
        {
            IsBusy = true;
            try
            {
                RestServiceResponse response = await RestService.LockSeats(Event, SelectedTarrrifs);
                if(! response.IsSuccess)
                {
                    DisplayAlert("Error", response.ErrorMessage, "Close");
                    return;
                }
                Booking = response.Data.ToObject<Booking>();
                Booking.Tarriffs = SelectedTarrrifs;
                List<CartItem> cartItems = new List<CartItem>();
                for (int i = 0; i<Booking.Seats?.Count; i++)
                {
                    BookingSeat seat = Booking.Seats[i];
                    Tarriff tarriff = SelectedTarrrifs.FirstOrDefault(t => t.ID == seat.tariffId);
                    CartItem cartItem = new CartItem
                    {
                        Quantity = 1,
                        Price = tarriff.price.GetValueOrDefault(),
                        Total = tarriff.price.GetValueOrDefault(),
                        Name = string.Format ("{0} - {1}", tarriff.displayName, seat.areaCode),
                        Currency = tarriff.currency,
                        CurrencyName = tarriff.CurrencyName,
                        Tarriff = tarriff

                    };
                    cartItems.Add(cartItem);
                }

                CartItems = cartItems.GroupBy(t=> t.Tarriff).Select(cartiItem => new CartItem {
                    Quantity = cartiItem.Count(),
                    Price = cartiItem.First().Price,
                    Total = cartiItem.Sum(ci => ci.Price),
                    Name = cartiItem.First().Name,
                    Currency = cartiItem.First().Currency,
                    CurrencyName = cartiItem.First().CurrencyName,
                    Tarriff = cartiItem.First().Tarriff
                }).ToList();

                Log.WriteLine("Booking: " + Booking.bookingRef);
                OnBookingCompleted(new BookingEventArgs { Booking = Booking });
            }
            catch (Exception e)
            {
                Log.WriteLine("Error locking seats: " + e.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void ExecutePayBookingCommand()
        {
            IsBusy = true;
            try
            {
                RestServiceResponse response = await RestService.PrepareOrder(Booking);
                if (!response.IsSuccess)
                {
                    DisplayAlert("Error", response.ErrorMessage, "Close");
                    return;
                }

               
                PaymentPage payment = new PaymentPage(SettingsViewModel.BankPosModel, Booking.Total.totalPrice.GetValueOrDefault(), "RON");
                var result = await ShowPopup(payment);
                Log.WriteLine("payed with success" + result);

                Log.WriteLine("ExecutePayBookingCommand response: " + response.ResponseString);
              
            }
            catch(Exception e)
            {
                Log.WriteLine("Error paying booking: " + e.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void RefreshCanExecutes()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                (IncrementCommand as Command).ChangeCanExecute();
                (DecrementCommand as Command).ChangeCanExecute();
                (LockSeatsCommand as Command).ChangeCanExecute();
                (PayBookingCommand as Command).ChangeCanExecute();

            });
        }

       


    }
}
