using iaBilet.Resources.Strings;

using CommunityToolkit.Maui.Views;
using iaBilet.Core.Lib;
using iaBilet.Pos.Provider;


namespace iaBilet.Pos;

public partial class PaymentPage : Popup
{
	PosViewModel viewModel;
	public PaymentPage(PosViewModel _viewModel)
	{
		BindingContext = viewModel = _viewModel;
        this.Opened += PaymentPage_Opened;
        this.Closed += PaymentPage_Closed;
		InitializeComponent();
	}

    public PaymentPage(PosModel posModel, decimal Amount, string Currency)
    {
        BindingContext = viewModel = Application.Current.Handler.MauiContext.Services.GetService<PosViewModel>();
        viewModel.PosModel = posModel;
        viewModel.Title = string.Format("{0} {1}{2}", AppResources.Pay, Amount, Currency);
        viewModel.Message = AppResources.PleaseWait;
        viewModel.Amount = Amount;
        viewModel.Currency = Currency;
        

        this.Opened += PaymentPage_Opened;
        this.Closed += PaymentPage_Closed;
        InitializeComponent();
    }

    private async void PaymentPage_Closed(object? sender, CommunityToolkit.Maui.Core.PopupClosedEventArgs e)
    {
        viewModel.Driver.ResponseReceived -= Driver_ResponseReceived;
    }

    private async void PaymentPage_Opened(object? sender, CommunityToolkit.Maui.Core.PopupOpenedEventArgs e)
    {
        viewModel.Driver.ResponseReceived += Driver_ResponseReceived;
        await viewModel.Driver.Pay();
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        await Task.Delay(2000);
        await CloseAsync(viewModel.Driver, cts.Token);

    }

    private void Driver_ResponseReceived(object source, PosResponseEventArgs e)
    {
        Log.WriteLine("Driver_ResponseReceived");
        viewModel.Driver_ResponseReceived(source, e);
    }
}