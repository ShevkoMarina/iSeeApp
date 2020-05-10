using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using MyApp.Views.Detail;
using MyApp.Views.Hints;
using System;
using MyApp.RecognitionClasses;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using MyApp.ViewModels.Navigation;
using MyApp.Views.ErrorAndEmpty;

namespace MyApp.Views.Navigation
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigationListCardPage
    {
        private static SpeechConfig speechConfig;

        public static SpeechConfig SpeechConfig { get => speechConfig; set => speechConfig = value; }

        public NavigationListCardPage()
        {
            InitializeComponent();
            SpeechConfig = SpeechConfig.FromSubscription(Constants.SpeechKey, "eastus");
            this.BindingContext = new NavigationViewModel().FunctionsList[0];   
        }

        /// <summary>
        /// Отменяет озвучку при выходе со страницы
        /// </summary>
        protected override void OnDisappearing()
        {
            SpeechSyntezer.CancelSpeech();
        }

        /// <summary>
        /// Открывает страницу обучения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TipsButtonClicked(object sender, EventArgs e)
        {        
            await Navigation.PushAsync(new OnBoardingAnimationPage());
            await SpeechSyntezer.VoiceResult("Выберите в главном меню нужную функцию распознавания");
        }

        /// <summary>
        /// Записывает голосовую команду
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void VoiceButtonClicked(object sender, EventArgs e)
        {
           
            VoiceButton.IsEnabled = false;
            VoiceButton.Source = "MicroRecording.png";
            if (await AudioRecording.CheckAudioPermissions())
            {
                string filePath = await AudioRecording.RecordAudio();
                await AnalizeAudioCommand(filePath);
            }
            VoiceButton.IsEnabled = true;
            VoiceButton.Source = "micro.png";       
        }

        /// <summary>
        /// Определяет назначение голосовой команды из 2 слов
        /// </summary>
        /// <param name="commandPage"></param>
        /// <param name="commandCamera"></param>
        /// <returns></returns>
        private async Task CheckCommandsPage(string commandPage, string commandCamera)
        {
            switch (commandPage.Substring(0,5))
            {
                case "деньг":
                    var BP = new BanknotesRecognitionPage();
                    await Navigation.PushAsync(BP);
                    await BP.VoiceCommand(commandCamera);
                    break;
                case "печат":
                    var PP = new RecognitionPrintedPage();
                    await Navigation.PushAsync(new RecognitionPrintedPage());
                    await PP.VoiceCommand(commandCamera);
                    break;
                case "рукоп":
                    var RP = new RecognitionHandwrittenPage();
                    await Navigation.PushAsync(RP);
                    await RP.VoiceCommand(commandCamera);
                    break;
                case "помощ":
                    await Navigation.PushAsync(new OnBoardingAnimationPage());
                    break;
                default:
                    await SpeechSyntezer.VoiceResult("Такой команды не существует.");
                    break;
            }
        }

        /// <summary>
        /// Определяет назначение голосовой команды из 1 слова
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private async Task CheckCommandsPageOneCommand(string command)
        {
            switch (command.Substring(0,5))
            {
                case "деньг":
                    await Navigation.PushAsync(new BanknotesRecognitionPage());                
                    break;
                case "печат":
                    await Navigation.PushAsync(new RecognitionPrintedPage());
                    break;
                case "рукоп":
                    await Navigation.PushAsync(new RecognitionHandwrittenPage());
                    break;
                case "помощ":
                     break;
                default:
                    await SpeechSyntezer.VoiceResult("Такой команды не существует.");
                    break;
            }          
        }


        /// <summary>
        /// Анализирует голосовую команду
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task AnalizeAudioCommand(string filePath)
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

                            var result = await recognizer.RecognizeOnceAsync();
                            if (result.Reason == ResultReason.RecognizedSpeech)
                            {
                                string[] commands = PreprocessingCommands(result);
                                switch(commands.Length)
                                {
                                    case 0:
                                        await SpeechSyntezer.VoiceResult("Речь не распознана");
                                        break;
                                    case 1:
                                        await CheckCommandsPageOneCommand(commands[0]);
                                        break;
                                    case 2:
                                        await CheckCommandsPage(commands[0], commands[1]);
                                        break;
                                    default:
                                        await SpeechSyntezer.VoiceResult("Такой команды не существует");
                                        break;

                                }           
                            }
                            else
                            {
                                await SpeechSyntezer.VoiceResult("Речь не распознана");
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                await Navigation.PushAsync(new SomethingWentWrongPage());
            }
        }

        /// <summary>
        /// Предобработка голосовых команд
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private static string[] PreprocessingCommands(SpeechRecognitionResult result)
        {
            string command = result.Text.ToLower();
            command = command.Replace(".", "").Replace("?", "").Replace("!", "").Replace(",", "");
            string[] commands = command.Split(' ');
            return commands;
        }
    }
}
