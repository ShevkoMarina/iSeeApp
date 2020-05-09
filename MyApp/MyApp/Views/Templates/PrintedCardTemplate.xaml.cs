using System;
using MyApp.ViewModels.Navigation;
using MyApp.Views.Detail;
using MyApp.Views.ErrorAndEmpty;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace MyApp.Views.Templates
{

    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrintedCardTemplate : Grid
    {
		public PrintedCardTemplate()
        {
            InitializeComponent();
            this.BindingContext = new NavigationViewModel().FunctionsList[0];
        }

        private async void PrintedTextItem_Clicked(object sender, EventArgs e)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                await Navigation.PushAsync(new RecognitionPrintedPage());
            }
            else
            {
                await Navigation.PushAsync(new NoInternetConnectionPage());
            }
        }
    }
}