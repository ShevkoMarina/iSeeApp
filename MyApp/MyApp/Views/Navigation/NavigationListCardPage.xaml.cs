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
using Xamarin.Forms;
using Xamarin.Essentials;

namespace MyApp.Views.Navigation
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigationListCardPage
    {

        public NavigationListCardPage()
        {
            InitializeComponent();
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
        /// Запускает голосовое управление
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void VoiceButtonClicked(object sender, EventArgs e)
        {
            try
            {
                Vibration.Vibrate();
                BottomPanel.IsEnabled = false;
                VoiceButton.IsEnabled = false;
                BottomBoxView.BackgroundColor = Color.FromHex("#C00000");
                if (await AudioRecording.CheckAudioPermissions())
                {
                    await AudioRecording.RecordAudio();
                    await AnalizeAudioCommand(AudioRecording.RecorderPath);
                }
                BottomPanel.IsEnabled = true;
                VoiceButton.IsEnabled = true;
                BottomBoxView.BackgroundColor = Color.Black;
            }
            catch (Exception)
            {
                await Navigation.PushAsync(new SomethingWentWrongPage());
            }
            finally
            {
                BottomPanel.IsEnabled = true;
                VoiceButton.IsEnabled = true;
                BottomBoxView.BackgroundColor = Color.Black;
            }
        }


        /// <summary>
        /// Выполняет задачу заданную голосом на главной странице
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private async Task DoCommandActionOnMain(string command)
        {
            if (command.Length < 5)
            {
                await SpeechSyntezer.VoiceResult("Такой команды не существует.");
            }
            else
            {
                if (command.Contains("банк"))
                {
                    await Navigation.PushAsync(new BanknotesRecognitionPage());
                    return;
                }
                if (command.Contains("печат"))
                {
                    await Navigation.PushAsync(new RecognitionPrintedPage());
                    return;
                }
                if (command.Contains("рукоп"))
                {
                    await Navigation.PushAsync(new RecognitionHandwrittenPage());
                    return;
                }
                if (command.Contains("помощ"))
                {
                    await Navigation.PushAsync(new OnBoardingAnimationPage());
                    await SpeechSyntezer.VoiceResult("Выберите в главном меню нужную функцию распознавания");
                    return;
                }
                else
                {
                    await SpeechSyntezer.VoiceResult("Такой команды не существует");

                }
            }
        }

        /// <summary>
        /// Преобразует речь в текст и запускает выполнение команды
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task AnalizeAudioCommand(string filePath)
        {
            if (!String.IsNullOrEmpty(filePath))
            {
                using (var audioInput = AudioConfig.FromWavFileInput(filePath))
                {
                    using (var recognizer = new SpeechRecognizer(SpeechAnalyzer.SpeechConfiguration, audioInput))
                    {
                        var result = await recognizer.RecognizeOnceAsync();
                        if (result.Reason == ResultReason.RecognizedSpeech)
                        {
                            if (String.IsNullOrEmpty(result.Text))
                            {
                                await SpeechSyntezer.VoiceResult("Не удалось распознать речь");
                            }
                            else
                            {
                                string processedText = SpeechAnalyzer.PreprocessingCommands(result.Text);
                                await DoCommandActionOnMain(processedText);
                            }
                        }
                        else
                        {
                            await SpeechSyntezer.VoiceResult("Не удалось распознать речь");
                        }
                    }
                }
            }
        }
    } 
}
