using CommunityToolkit.Maui.Views;
using iaBilet.Core.Lib;
using iaBilet.ViewModels;
namespace iaBilet;

public partial class EventPopup : Popup
{
    MainPageViewModel ViewModel;
	public EventPopup()
	{
		BindingContext = ViewModel = Application.Current.Handler.MauiContext.Services.GetService<MainPageViewModel>();
        this.Opened += async (s, e) =>
        {
            ViewModel.IsBusy = true;
            var a = MainContainer.ScaleTo(1.0);
            var b = MainContainer.FadeTo(1.0);
            await Task.WhenAll(a, b);


            try
            {
                await ViewModel.CartViewModel.LoadTarrifs();
            }
            catch (Exception ex)
            {
                Log.WriteLine("Error loading tarrifs: " + ex.Message);
            }
            finally
            {
                ViewModel.IsBusy = false;
            }
        };
        InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        //1Command = "{Binding CloseEventViewCommand}"
        var a = MainContainer.ScaleTo(0.0);
        var b = MainContainer.FadeTo(0.0);
        await Task.WhenAll(a, b);
        this.Close();
    }
}