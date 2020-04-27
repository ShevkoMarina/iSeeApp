using MyApp.DataService;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using MyApp.Views.Detail;
using MyApp.Views.Hints;
using System;
using MyApp.RecognitionClasses;
using System.Threading.Tasks;
using MyApp.RecognitionClasses.CameraClass;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System.Collections.Generic;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace MyApp.Views.Navigation
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigationListCardPage
    {
        public NavigationListCardPage()
        {
            InitializeComponent();
            SpeechConfig = SpeechConfig.FromSubscription(Constants.SpeechKey, "eastus");
            this.BindingContext = NavigationDataService.Instance.NavigationViewModel;
         
        }
        private async void BanknotesItem_Clicked(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {    
            await Navigation.PushAsync(new BanknotesRecognitionPage());
        }
        private async void PrintedTextItem_Clicked(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {         
            await Navigation.PushAsync(new RecognitionPrintedPage());          
        }
        private async void HandwrittenTextItem_Clicked(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new RecognitionHandwrittenPage());
        }

        private async void SettingsButtonClicked(object sender, EventArgs e)
        {
            
        }

        private async void TipsButtonClicked(object sender, EventArgs e)
        {
         
            await Navigation.PushAsync(new OnBoardingAnimationPage());
            await TextSyntezer.VoiceResult("Выберите в главном меню нужную функцию распознавания");
        }

        private async void VoiceButtonClicked(object sender, EventArgs e)
        {
            if (await AudioRecording.CheckAudioPermissions())
            {
                string path = await AudioRecording.RecordAudio();
                await AnalizeCommand(path);
            }
        }


        private async Task CheckCommandsPage(string commandPage, string commandCamera)
        {
            switch (commandPage)
            {
                case "деньги":
                    var BP = new BanknotesRecognitionPage();
                    await Navigation.PushAsync(BP);
                    await BP.VoiceCommand(commandCamera);
                    break;
                case "печатный":
                    var PP = new RecognitionPrintedPage();
                    await Navigation.PushAsync(new RecognitionPrintedPage());
                    await PP.VoiceCommand(commandCamera);
                    break;
                case "рукописный":
                    var RP = new RecognitionHandwrittenPage();
                    await Navigation.PushAsync(RP);
                    await RP.VoiceCommand(commandCamera);
                    break;
                case "помощь":
                    break;
                default:
                    await TextSyntezer.VoiceResult("Такой команды не существует.");
                    break;
            }
        }

        private async Task CheckCommandsPageOneCommand(string command)
        {
            
            switch (command)
            {
                case "деньги":
                    await Navigation.PushAsync(new BanknotesRecognitionPage());                
                    break;
                case "печатный":
                    await Navigation.PushAsync(new RecognitionPrintedPage());
                    break;
                case "рукописный":
                    await Navigation.PushAsync(new RecognitionHandwrittenPage());
                    break;
                case "помощь":
                     break;
                default:
                    await TextSyntezer.VoiceResult("Такой команды не существует.");
                    break;
            }          
        }

        public static SpeechConfig SpeechConfig;

        public async Task AnalizeCommand(string filePath)
        {
            try
            {
                if (!String.IsNullOrEmpty(filePath))
                {       
                    SpeechConfig.SpeechRecognitionLanguage = "ru-RU";
                    using (var audioInput = AudioConfig.FromWavFileInput(filePath))
                    {
                        using (var recognizer = new SpeechRecognizer(SpeechConfig, audioInput))
                        {          
                           
                            PhraseListGrammar phraseList = PhraseListGrammar.FromRecognizer(recognizer);
                            phraseList.AddPhrase("камера");

                        
                            var result = await recognizer.RecognizeOnceAsync();
                            if (result.Reason == ResultReason.RecognizedSpeech)
                            {
                                string command = result.Text.ToLower();
                                command = command.Replace(".", "").Replace("?", "").Replace("!", "").Replace(",", "");
                                string[] commands = command.Split(' ');
                                await DisplayAlert("sdfs", commands[0], "ok");
                                if (commands.Length > 1) await CheckCommandsPage(commands[0], commands[1]);
                                else await CheckCommandsPageOneCommand(commands[0]);
                            }
                            else
                            {
                                await TextSyntezer.VoiceResult("Речь не распознана");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
        
    }
}
