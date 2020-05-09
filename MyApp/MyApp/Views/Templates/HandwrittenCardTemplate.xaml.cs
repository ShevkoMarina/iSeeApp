using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyApp.Views.Detail;
using Xamarin.Essentials;
using MyApp.Views.ErrorAndEmpty;
using MyApp.ViewModels.Navigation;

namespace MyApp.Views.Templates
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HandwrittenCardTemplate : Grid
    {
        public HandwrittenCardTemplate()
        {
            InitializeComponent();
            this.BindingContext = new NavigationViewModel().FunctionsList[2];
        }

        /// <summary>
        /// Открывает функцию распознавания рукописного текста
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void HandwrittenTextItem_Clicked(object sender, EventArgs e)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                await Navigation.PushAsync(new RecognitionHandwrittenPage());
            }
            else
            {
                await Navigation.PushAsync(new NoInternetConnectionPage());
            }
        }
    }
}