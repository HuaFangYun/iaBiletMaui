namespace iaBilet.Settings.Views;
using iaBilet.ViewModels.Settings;
public partial class General : ContentPage
{
	SettingsViewModel viewModel;
	public General()
	{
		BindingContext = viewModel = Application.Current.Handler.MauiContext.Services.GetService<SettingsViewModel>();
        InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
}