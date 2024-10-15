using iaBilet.Core.Lib;

namespace iaBilet.Views.Pages;

public partial class EventPage : ContentPage
{
    MainPageViewModel ViewModel;
    public EventPage()
	{
        BindingContext = ViewModel = Application.Current.Handler.MauiContext.Services.GetService<MainPageViewModel>();
        InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        ViewModel.IsBusy = true;
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
    }

   
}