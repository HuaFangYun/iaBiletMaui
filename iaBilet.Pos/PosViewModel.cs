using iaBilet.Core.Lib;
using iaBilet.Core.Services;
using iaBilet.Pos.Provider;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace iaBilet.Pos
{
    public class PosViewModel : ViewModel
    {

       
        public static PosViewModel Instance
        {
            get => Application.Current.Handler.MauiContext.Services.GetService<PosViewModel>();
        }


        private const string BLE_CONECTIVITY_TYPE = "BLUETHOOT";
        private const string SERIAL_CONECTIVITY_TYPE = "SERIAL";
        private const string NETWORK_CONECTIVITY_TYPE = "TCPIP";

        public static string[] AvailableConnectivityTypes
        {
            get => new string[] { BLE_CONECTIVITY_TYPE, SERIAL_CONECTIVITY_TYPE, NETWORK_CONECTIVITY_TYPE };
        }
        public static List<PosModel> AvailableBankPosModels
        {
            get => GetAvailablePosDevices();
        }

        public static List<PosModel> GetAvailablePosDevices()
        {
            List<PosModel> devices = new List<PosModel>();
            Type[] typeList = Helpers.GetTypesInNamespace(Assembly.GetExecutingAssembly(), "iaBilet.Pos.Devices");
            for (int i = 0; i < typeList.Length; i++)
            {

                Log.WriteLine("type name" + typeList[i].Name);
                PosModel device = Activator.CreateInstance(typeList[i]) as PosModel;
                devices.Add(device);
                Log.WriteLine("added device name" + device.Name);
            }

            return devices;
        }

        private PosModel _posModel;
        public PosModel PosModel
        {
            get { return _posModel; }
            set => SetProperty(ref _posModel, value);
        }

        public double WindowSize => Math.Min(MainDisplayHeight, MainDisplayWidth);
        private decimal _amount;
        public decimal Amount
        {
            get { return _amount; }
            set => SetProperty(ref _amount, value);
        }

        private string _currency;
        public string Currency
        {
            get => _currency;
            set => SetProperty(ref _currency, value);
        }

        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        private bool _shouldPrintBankReceipt = true;
        public bool ShouldPrintBankReceipt
        {
            get => _shouldPrintBankReceipt;
            set => SetProperty(ref _shouldPrintBankReceipt, value);
        }

        private bool _transactionFinishedWithSuccess = false;
        public bool TransactionFinishedWithSuccess
        {
            get { return _transactionFinishedWithSuccess; }
            set
            {
                SetProperty(ref _transactionFinishedWithSuccess, value);
            }
        }

        public PosProvider Driver
        {
            get  {
                PosProvider driver =  PosModel.Driver;
                driver.Amount = Amount;
                return driver;
            }
            /*
            get
            {
                Castles c = new Castles(PosModel);
                c.Amount = Amount;
                c.Currency = Currency;
                c.ResponseReceived += Driver_ResponseReceived;
                return c;
            }
            */
        }


        public void Driver_ResponseReceived(object source, PosResponseEventArgs e)
        {
           Log.WriteLine("Driver_ResponseReceived");
            PosResponse response = e.GetResponse();
            Type t = e.GetResponseType();
            this.SetResponse(response);
        }

        public void SetResponse(PosResponse response)
        {
            Log.WriteLine("SetResponse " + response.Message + "is hold" + response.IsHold + "isEreror" + response.IsError);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                bool closeWithoutError = true;
                Message = response.Message.Trim();

                if (response.IsHold)
                {
                    //this.CloseWindow.Visibility = Visibility.Collapsed;
                    //this.PosMessage.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2875BD"));
                    Log.WriteLine("Return on hold");
                    return;
                }

                if (response.IsError || !Driver.TransactionFinshedWithSuccess)
                {
                    Log.WriteLine("Return error");
                    SetErrorTheme();
                    //  closeWithoutError = false;
                    //  this.SetErrorMessage(response.Message);
                    //   this.SelfClose(true, closeWithoutError);
                    return;
                }
                Log.WriteLine("Return final");
                SetSuccessTheme();
                TransactionFinishedWithSuccess = true;
            });
        }

    }
}
