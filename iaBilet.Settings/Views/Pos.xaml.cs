using iaBilet.ViewModels.Settings;
using iaBilet.Pos;
using iaBilet.Core.Lib;

namespace iaBilet.Settings.Views;

public partial class Pos : ContentPage
{
    SettingsViewModel viewModel;
    public Pos()
	{
        BindingContext = viewModel = Application.Current.Handler.MauiContext.Services.GetService<SettingsViewModel>();
        InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.LoadPosSettings();
    }

    private async void TestPayment_Clicked(object sender, EventArgs e)
    {
        PaymentPage payment = new PaymentPage(viewModel.BankPosModel, 5, "RON");
        var result = await viewModel.ShowPopup(payment);
        Log.WriteLine("payed with success" + result);
    }
}