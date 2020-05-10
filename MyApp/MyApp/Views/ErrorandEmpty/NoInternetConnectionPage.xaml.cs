using MyApp.RecognitionClasses;
using System;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace MyApp.Views.ErrorAndEmpty
{
    /// <summary>
    /// Page to show the no internet connection error
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoInternetConnectionPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoInternetConnectionPage" /> class.
        /// </summary>
        public NoInternetConnectionPage()
        {
            InitializeComponent();
            SpeechSyntezer.VoiceResult("Нет Интернета");
        }

        /// <summary>
        /// Повтроная проверка подключения к интернету
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TryAgain_Clicked(object sender, EventArgs e)
        {
            if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            {
                await SpeechSyntezer.VoiceResult("Нет Интернета");
            }
            else
            {
                await Navigation.PopToRootAsync();
            }
        }

        /// <summary>
        /// Отменяет озвучку при выходе со страницы
        /// </summary>
        protected override void OnDisappearing()
        {
            SpeechSyntezer.CancelSpeech();
        }
    }
}