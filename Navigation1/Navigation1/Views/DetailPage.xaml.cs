using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Navigation1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailPage : ContentPage
    {
        public DetailPage()
        {
            InitializeComponent();
        }

        private void DisappearPage(object sender, EventArgs e)
        {
            var target = BindingContext as ViewModels.DetailViewModel;
            if (target != null) {
                target.Disappear.Execute(null);
            }
        }
    }
}