using iaBilet.Pos.Provider;
using iaBilet.Pos.Provider.Castles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iaBilet.Pos.Devices
{
    public class CastleUPT100 : PosModel
    {
        public override string Name => "CastleUPT100(Libra)";
        public override string Code => "CSTLLBK";
        public override string ConectivityType => SERIAL_CONECTIVITY_TYPE;
        public override string DriverName => "CastleUPT100";
        public override int BaudRate => 115200;
        protected override PosProvider InitializeDriver()
        {
            Castles drv = new Castles();
            drv.Port = Port;
            drv.BaudRate = BaudRate;
            return drv;
        }
    }
}
