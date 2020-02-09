using System;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using Plugin.Media.Abstractions;
using Plugin.Media;
using Plugin.Permissions.Abstractions;
using Plugin.Permissions;
using System.Linq.Expressions;
using System.Text;
using Xamarin.Forms;
using Plugin.AudioRecorder;
using System.Collections;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using MyApp.Views.Navigation;
using MyApp.Views.ErrorAndEmpty;

namespace MyApp
{
    // При возникновении исключения пересылать на страницу Error

    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        #region MainPage static fields
        public static AudioRecorderService recorder;
        public static SpeechConfig EngSpeechConf;
        #endregion
        public MainPage()
        {
            InitializeComponent();

            EngSpeechConf = SpeechConfig.FromSubscription(Classes.Constants.SpeechKey, "eastus");
            if (SettingsPage.Language == "Russian")
            {
                settings.Text = "Настройки";
                textButton.Text = "Рукописный";
                pricelist.Text = "Печатный";
                banknotes.Text = "Банкноты";
            }

            recorder = new AudioRecorderService
            {
                StopRecordingAfterTimeout = true,
                TotalAudioTimeout = TimeSpan.FromSeconds(10),
                AudioSilenceTimeout = TimeSpan.FromSeconds(3)
            };
        }
        private async Task RecordAudio2()
        {

            try
            {
                if (!recorder.IsRecording) //Record button clicked
                {
                    RecordButton.IsEnabled = false;

                    var audioRecordTask = await recorder.StartRecording();
                    RecordButton.IsEnabled = true;
                    await audioRecordTask;
                    //await DisplayAlert("INFO", "Succsess recording", "OK");
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Occured", "OK");
            }
        }
        private async Task RecordAudio()
        {
            try
            {
                if (!recorder.IsRecording) //Record button clicked
                {
                    RecordButton.IsEnabled = false;

                    var audioRecordTask = await recorder.StartRecording();
                    RecordButton.IsEnabled = true;
                    await audioRecordTask;
                }
                else //Stop button clicked
                {
                    RecordButton.IsEnabled = false;

                    await recorder.StopRecording();
                    RecordButton.IsEnabled = true;
                    //await DisplayAlert("INFO", "Succsess recording", "OK");
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Occured", "OK");
            }
        }
        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }

        private async void StartRecorder_Clicked(object sender, EventArgs e)
        {
            await RecordAudio2();
            await AnalyzeAudio();
        }

        // Привет
        public async void ReadPriceList(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PriceListPage());
        }

        private async Task AnalyzeAudio()
        {
            try
            {
                var filePath = recorder.GetAudioFilePath();

                if (filePath != null)
                {

                    RecordButton.IsEnabled = false;
                    EngSpeechConf.SpeechRecognitionLanguage = "en-US";
                    using (var audioInput = AudioConfig.FromWavFileInput(filePath))
                    {
                        using (var recognizer = new SpeechRecognizer(EngSpeechConf, audioInput))
                        {

                            PhraseListGrammar phraseList = PhraseListGrammar.FromRecognizer(recognizer);
                            phraseList.AddPhrase("Pricelist.");
                            phraseList.AddPhrase("Camera.");
                            phraseList.AddPhrase("Gallery.");

                            //await DisplayAlert("Results", "Recognizing first result...", "OK");
                            var result = await recognizer.RecognizeOnceAsync();

                            if (result.Reason == ResultReason.RecognizedSpeech)
                            {
                                //await DisplayAlert("Results", $"We recognized: {result.Text}", "OK");

                                await CheckCommandsForMain(result.Text);
                            }
                            else if (result.Reason == ResultReason.NoMatch)
                            {
                                await DisplayAlert("Results", $"NOMATCH: Speech could not be recognized.", "OK");
                            }
                            else if (result.Reason == ResultReason.Canceled)
                            {
                                var cancellation = CancellationDetails.FromResult(result);
                                await DisplayAlert("Results", $"CANCELED: Reason={cancellation.Reason}", "OK");

                                if (cancellation.Reason == CancellationReason.Error)
                                {
                                    await DisplayAlert("Error", $"CANCELED: ErrorCode={cancellation.ErrorCode}", "OK");
                                    await DisplayAlert("Error", $"ErrorDetails ={cancellation.ErrorDetails}", "OK");

                                }
                            }

                            RecordButton.IsEnabled = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Occured", "OK");
            }
        }

      
        public async void Meth()
        {
            await Navigation.PushAsync(new PriceListPage());
        }
        private async Task CheckCommandsForMain(string command)
        {
            if (!Classes.TextDetector.AnyErrors)
            {
                switch (command)
                {
                    case "Pricelist.":
                        await Navigation.PushAsync(new PriceListPage());
                        break;                    
                    case "Money.":
                        break;
                    case "Text.":
                        break;
                    case "Camera":
                       // await CameraActions.TakePhoto();
                        break;
                    case "Gallery":
                       // await CameraActions.GetPhoto();
                        break;
                    default:
                        await Classes.TextSyntezer.SpeakResult("This command doesn't exist.");
                        break;
                  
                }
            }
        }
       
        private async Task CheckAudioPermissions()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Microphone);
            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Microphone))
                {

                }
                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Microphone);
                if (results.ContainsKey(Permission.Microphone))
                {
                    status = results[Permission.Microphone];
                }
            }
        }
        
        private async void banknotes_Clicked(object sender, EventArgs e)
        {
            if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            {
                await Navigation.PushAsync(new NoInternetConnectionPage());
            }
            else
            {
                await Navigation.PushAsync(new NavigationListCardPage());
            }
        }

        private async void textButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NoInternetConnectionPage());
        }
    }
}

