using iaBilet.ViewModels;

namespace iaBilet
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        MainPageViewModel ViewModel;
        public MainPage(MainPageViewModel viewModel)
        {
           BindingContext =  ViewModel = viewModel;
           InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.IsBusy = true;
            try
            {
                await ViewModel.LoadData();
            }
            catch (Exception ex) { }
            finally
            {
                ViewModel.IsBusy = false;
            }
        }
    }
}
