using System.ComponentModel;
using Xamarin.Forms;
using xamarin1.ViewModels;

namespace xamarin1.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}