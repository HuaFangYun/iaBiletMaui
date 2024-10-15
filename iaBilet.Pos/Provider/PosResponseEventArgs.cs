using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iaBilet.Pos.Provider
{
    public class PosResponseEventArgs
    {
        private PosResponse _response;
        private Type _responseType;


        public PosResponseEventArgs(PosResponse response, Type responseType = null)
        {
            _response = response;
            _responseType = responseType;
        }

        public PosResponse GetResponse()
        {
            return _response;
        }
        public Type GetResponseType()
        {
            return _responseType;
        }
    }
}
