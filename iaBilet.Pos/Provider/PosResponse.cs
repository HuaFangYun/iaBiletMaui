using iaBilet.Core.Lib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iaBilet.Pos.Provider
{
    public class PosResponse : INotifyObject
    {
        private string _message = "";
        public virtual string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private string _hexResponse = "";
        public string HexResponse
        {
            get { return _hexResponse; }
            set { SetProperty(ref _hexResponse, value); }
        }
        private bool _isError = false;
        public bool IsError
        {
            get { return _isError; }
            set { SetProperty(ref _isError, value); }
        }

        private bool _isHold = false;
        public bool IsHold
        {
            get { return _isHold; }
            set { SetProperty(ref _isHold, value); }
        }

        private bool _isSuccess = false;
        public bool IsSucces
        {
            get { return _isSuccess; }
            set { SetProperty(ref _isSuccess, value); }
        }

        private bool _payedWithSuccess = false;
        public bool PayedWithSuccess
        {
            get { return _payedWithSuccess; }
            set { SetProperty(ref _payedWithSuccess, value); }
        }
      
    }
}
