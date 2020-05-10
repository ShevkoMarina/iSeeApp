using MyApp.RecognitionClasses;
using System;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace MyApp.Views.ErrorAndEmpty
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SomethingWentWrongPage
    {
        public SomethingWentWrongPage()
        {
            InitializeComponent();
            SpeechSyntezer.VoiceResult("Произошла ошибка");
        }

        /// <summary>
        /// Возвращает на главную страницу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void GoBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
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