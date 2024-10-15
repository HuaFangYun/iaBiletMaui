namespace iaBilet;

using CommunityToolkit.Maui.Views;
using iaBilet.Resources.Strings;
public partial class FinalizePopup : Popup
{
    MainPageViewModel ViewModel;
    public FinalizePopup()
	{
        BindingContext = ViewModel = Application.Current.Handler.MauiContext.Services.GetService<MainPageViewModel>();
        InitializeComponent();
	}
    /*
    protected  async void OnAppearing()
    {
        base.OnAppearing();
        ViewModel.CartViewModel.RefreshCanExecutes();

    }
    */
}