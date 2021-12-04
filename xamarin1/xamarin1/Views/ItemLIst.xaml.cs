using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xamarin1.ViewModels;
using System.Diagnostics;

namespace xamarin1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemList : ContentPage
    {
        ItemsViewModel viewModel_;

        public ItemList()
        {
            InitializeComponent();
            BindingContext = viewModel_ = new ItemsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel_.OnAppearing();
        }
    }
}