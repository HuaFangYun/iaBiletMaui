using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using iaBilet.Pos;
using iaBilet.Core.Lib;


namespace iaBilet.ViewModels.Settings
{
    public partial class SettingsViewModel
    {

        public string[] PosAvailableConnectivityTypes
        {
            get => PosViewModel.AvailableConnectivityTypes;
        }
        private bool _posIsActive = Preferences.Get("PosIsActive", false);
        public bool PosIsActive
        {
            get => _posIsActive;
            set { SetProperty(ref _posIsActive, value); Preferences.Set("PosIsActive", value); }
        }

        private string _posSelectedPortName;
        public string PosSelectedPortName
        {
            get => _posSelectedPortName;
            set
            {
                SetProperty(ref _posSelectedPortName, value);
                if (_posSelectedPortName != null)
                {
                    BankPosPort = Convert.ToInt32(_posSelectedPortName.Replace("COM", ""));
                }
            }
        }
        private string _bankPosConnectivityType;
        public string BankPosConnectivityType
        {
            get => _bankPosConnectivityType;
            set => SetProperty(ref _bankPosConnectivityType, value);
        }

        public string _bankPosModelCode;
        public string BankPosModelCode
        {
            get { return string.IsNullOrEmpty(_bankPosModelCode) ? Preferences.Get("BankPosModelCode", "") : _bankPosModelCode; }
            set { SetProperty(ref _bankPosModelCode, value); Preferences.Set("BankPosModelCode", _bankPosModelCode); }
        }

        public int _bankPosPort;
        public int BankPosPort
        {
            get { return _bankPosPort > 0 ? _bankPosPort : Preferences.Get("BankPosCoPort", 1);  }
            set { SetProperty(ref _bankPosPort, value); Preferences.Set("BankPosCoPort", _bankPosPort); }
        }

        public string _bankPosTcpPort;
        public string BankPosTcpPort
        {
            get { return string.IsNullOrEmpty(_bankPosTcpPort) ? Preferences.Get("BankPosTcpPort", "") : _bankPosTcpPort; }
            set { SetProperty(ref _bankPosTcpPort, value); Preferences.Set("BankPosTcpPort", _bankPosTcpPort); }
        }

        public int _bankPosBaudRate;
        public int BankPosBaudRate
        {
            get { return _bankPosBaudRate > 0 ? _bankPosBaudRate : Preferences.Get("BaudRate", 115200); }
            set {
                if (value > 0) {
                    SetProperty(ref _bankPosBaudRate, value); Preferences.Set("BaudRate", _bankPosBaudRate);
                }
            }
        }

        public string _bankPosHost;
        public string BankPosHost
        {
            get { return string.IsNullOrEmpty(_bankPosHost) ? Preferences.Get("Host", "") : _bankPosHost; }
            set { SetProperty(ref _bankPosHost, value); Preferences.Set("Host", _bankPosHost); }
        }

        private string _bankPosBleAddress;
        public string BankPosBleAddress
        {
            get => _bankPosBleAddress ?? Preferences.Get("BleAddress", "");
            set { SetProperty(ref _bankPosBleAddress, value); Preferences.Set("BleAddress", _bankPosBleAddress); }
        }


        private bool _isPosSerialDevice;
        public bool IsPosSerialDevice
        {
            get => _isPosSerialDevice;
            set => SetProperty(ref _isPosSerialDevice, value);
        }

        private bool _isPosNetworkDevice;
        public bool IsPosNetworkDevice
        {
            get => _isPosNetworkDevice;
            set => SetProperty(ref _isPosNetworkDevice, value);
        }

        private bool _isPosBluetoothDevice;
        public bool IsPosBluetoothDevice
        {
            get => _isPosBluetoothDevice;
            set => SetProperty(ref _isPosBluetoothDevice, value);
        }

        private bool _posAskForBoudRate;
        public bool PosAskForBoudRate
        {
            get => _posAskForBoudRate;
            set => SetProperty(ref _posAskForBoudRate, value);
        }


        private PosModel? _bankPosModel;
        public PosModel BankPosModel
        {
            get => _bankPosModel ?? AvailableBankPosModels.Where(t => t.Code == BankPosModelCode).FirstOrDefault();
            set { 
                SetProperty(ref _bankPosModel, value);
                if (value != null) {
                    BankPosModelCode = value.Code;
                }
            }
        }

        private List<PosModel> _availableBankPosModels =  new List<PosModel>();
        public List<PosModel> AvailableBankPosModels
        {
            get =>_availableBankPosModels;
            set=> SetProperty(ref _availableBankPosModels, value);
        }

        private string _posConnectivityType;
        public string PosConnectivityType
        {
            get => _posConnectivityType ?? Preferences.Get("PosConnectivityType", "");
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }
                SetProperty(ref _posConnectivityType, value); Preferences.Set("PosConnectivityType", _posConnectivityType);
                Log.WriteLine("set PosConnectivityType" + _posConnectivityType);

                IsPosSerialDevice = value == SERIAL_CONECTIVITY_TYPE;
                IsPosNetworkDevice = value == NETWORK_CONECTIVITY_TYPE;
                IsPosBluetoothDevice = value == BLE_CONECTIVITY_TYPE;
                PosAskForBoudRate = IsPosSerialDevice || IsPosBluetoothDevice;
            }
        }

        public void LoadPosSettings()
        {
            if (AvailableBankPosModels == null || AvailableBankPosModels.Count == 0)
            {
                AvailableBankPosModels = PosViewModel.AvailableBankPosModels;
            };
            PosSelectedPortName = BankPosPort > 0 ? "COM" + BankPosPort : "";
            if (BankPosModelCode != null)
            {
                BankPosModel = AvailableBankPosModels.Where(t => t.Code == BankPosModelCode).FirstOrDefault();
                if (BankPosModel != null) {
                    BankPosModel.Port = BankPosPort;
                    BankPosModel.BaudRate = BankPosBaudRate;
                    BankPosModel.Host = BankPosHost;
                    BankPosModel.TcpPort = BankPosTcpPort;
                    BankPosModel.BleAdress = _bankPosBleAddress;
                    PosViewModel.Instance.PosModel = BankPosModel;
                }
                if (! string.IsNullOrEmpty(PosConnectivityType) && PosAvailableConnectivityTypes.Contains(PosConnectivityType)) {
                    PosConnectivityType = PosAvailableConnectivityTypes[Array.IndexOf(PosAvailableConnectivityTypes, PosConnectivityType)];
                }
            }
#if WINDOWS
            AvailablePorts =  SerialPort.GetPortNames();
#endif
           
        }
    }
}
