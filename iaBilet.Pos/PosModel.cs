using iaBilet.Core.Lib;
using iaBilet.Pos.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iaBilet.Pos
{
    public class PosModel : INotifyObject
    {

        public const string BLE_CONECTIVITY_TYPE = "BLUETHOOT";
        public const string SERIAL_CONECTIVITY_TYPE = "SERIAL";
        public const string NETWORK_CONECTIVITY_TYPE = "TCPIP";

        public virtual string Name { set; get; }
        public virtual string Code { set; get; }
        public virtual string DriverName { set; get; }
        public virtual string ConectivityType { set; get; }
        public int Port { set; get; }
        public virtual int BaudRate { set; get; }
        public string Host { set; get; } //pt cele prin tcp ip
        public string TcpPort { set; get; } //pt cele prin tcp ip
        public string BleAdress { set; get; } //pt cele prin bluethoot

        static PosProvider _driver;
        public PosProvider Driver
        {
            get
            {
                if (_driver == null)
                {
                    _driver = InitializeDriver();
                }
                return _driver;
            }
        }

        protected virtual PosProvider InitializeDriver()
        {
            throw new NotImplementedException("InitializeDriver not implemented");
            return null;
        }
    }
}
