using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyApp.Views.Navigation;
using MyApp.RecognitionClasses;

namespace MyApp
{
    public partial class App : Application
    {
        public static string BaseImageUrl { get; } = "https://cdn.syncfusion.com/essential-ui-kit-for-xamarin.forms/common/uikitimages/";
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new NavigationListCardPage());
        }

        protected override void OnStart()
        {
            AuthenticationComputerVision.AuthenticateGoogleVision();
        }

        protected override void OnSleep()
        {
            SpeechSyntezer.CancelSpeech();
            SpeechSyntezer.CancelEnglishSpeech();
        }

        protected override void OnResume()
        {
           
        }
    }
}
