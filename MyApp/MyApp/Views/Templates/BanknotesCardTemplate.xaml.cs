using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyApp.Views.ErrorAndEmpty;
using MyApp.Models.Navigation;
using MyApp.ViewModels.Navigation;

namespace MyApp.Views.Templates
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BanknotesCardTemplate : Grid
    {
        public BanknotesCardTemplate()
        {
            InitializeComponent();
            this.BindingContext = new NavigationViewModel().FunctionsList[1];
        }

        private async void BanknotesRecognition_Clicked(object sender, EventArgs e)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                await Navigation.PushAsync(new BanknotesRecognitionPage());
            }
            else
            {
                await Navigation.PushAsync(new NoInternetConnectionPage());
            }
        }
    }
}