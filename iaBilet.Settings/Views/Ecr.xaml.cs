
using CommunityToolkit.Maui.Views;
using iaBilet.Core.Lib;
using iaBilet.ViewModels.Settings;
namespace iaBilet.Settings.Views;

public partial class Ecr : ContentPage
{
    SettingsViewModel viewModel;
    public Ecr()
	{
        BindingContext = viewModel = Application.Current.Handler.MauiContext.Services.GetService<SettingsViewModel>();
        InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    private async void ScanButton_Clicked(object sender, EventArgs e)
    {
       
    }
}