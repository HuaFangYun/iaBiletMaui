using iaBilet.Core.Lib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iaBilet.Pos.Provider
{
    public enum TransactionType
    {
        Sale = 1,
        Void = 2,
    };
    public delegate void ResponseHandler(object source, PosResponseEventArgs e);
    public class PosProvider : INotifyObject
    {

        public event ResponseHandler ResponseReceived;

        private decimal _amount;
        public decimal Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

        private string _currency = string.Empty;
        public string Currency
        {
            get => _currency;
            set => SetProperty(ref _currency, value);
        }

        private string _repsonse = string.Empty;
        public string Response
        {
            get => _repsonse;
            set => SetProperty(ref _repsonse, value);
        }

        private string _debugMessage = string.Empty;
        public string DebugMessage
        {
            get => _debugMessage;
            set => SetProperty(ref _debugMessage, value);
        }

        private string _userMessage = string.Empty;
        public string UserMessage
        {
            get => _userMessage;
            set => SetProperty(ref _userMessage, value);
        }

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private SolidColorBrush _textColor;
        public SolidColorBrush TextColor
        {
            get { return _textColor; }
            set => SetProperty(ref _textColor, value);
        }

        private SolidColorBrush _backgroundColor;
        public SolidColorBrush BackgroundColor
        {
            get { return _backgroundColor; }
            set => SetProperty(ref _backgroundColor, value);
        }

        private bool _shouldPrintBankReceipt = true;
        public bool ShouldPrintBankReceipt
        {
            get => _shouldPrintBankReceipt;
            set => SetProperty(ref _shouldPrintBankReceipt, value);
        }

        private bool _transactionFinshedWithSuccess = false;
        public bool TransactionFinshedWithSuccess
        {
            get { return _transactionFinshedWithSuccess; }
            set
            {
                SetProperty(ref _transactionFinshedWithSuccess, value);
                if (_transactionFinshedWithSuccess)
                {
                    ConfirmReceiptViewVisibility = Visibility.Visible;
                    WaitingViewVisibility = Visibility.Collapsed;
                }
            }
        }
        private Visibility _waitingViewVisibility = Visibility.Visible;
        public Visibility WaitingViewVisibility
        {
            get { return _waitingViewVisibility; }
            set { SetProperty(ref _waitingViewVisibility, value); }
        }

        private Visibility _confirmReceiptVisibility = Visibility.Collapsed;
        public Visibility ConfirmReceiptViewVisibility
        {
            get { return _confirmReceiptVisibility; }
            set { SetProperty(ref _confirmReceiptVisibility, value); }
        }

        private TransactionType _transactionType = TransactionType.Sale;
        public TransactionType TransactionType
        {
            get { return _transactionType; }
            set { SetProperty(ref _transactionType, value); }
        }

        private int _timoutSeconds = 120;
        public int TimoutSeconds
        {
            get => _timoutSeconds;
            set => SetProperty(ref _timoutSeconds, value);
        }

        public virtual Task Pay()
        {
            return Task.CompletedTask;
        }
        public virtual Task Abort()
        {
            return Task.CompletedTask;
        }

        public virtual Task Disconnect()
        {
            return Task.CompletedTask;
        }

        public void OnResponseReceived(PosResponseEventArgs e)
        {
            if (ResponseReceived != null)
            {
                ResponseReceived(this, e);//Raise the event
            }
        }

        public virtual void TriggerResponseReceived(bool error = false, string message = "")
        {
            return;
        }
    }
}
