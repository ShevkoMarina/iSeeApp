using System;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using MyApp.RecognitionClasses;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using MyApp.Views.ErrorAndEmpty;
using MyApp.RecognitionClasses.CameraClass;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech;

namespace MyApp.Views.Detail
{
   
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecognitionHandwrittenPage
    {
        public RecognitionHandwrittenPage()
        {
            InitializeComponent();
            SpeechSyntezer.VoiceResult("Рукописный текст");
        }
        private string detectedText;

        #region Methods

        /// <summary>
        /// Преобразует речь в текст и запускает выполнение команды
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private async Task AnalizeCommandHandwritten()
        {
            string filePath = AudioRecording.RecorderPath;
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
                                await DoCommandsActionOnHandwritten(processedText);
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

        /// <summary>
        /// Сделать фото, распознать рукописный текст и озвучить результат
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TakePhotoButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                TakePhotoButton.IsEnabled = false;
                var photo = await CameraActions.TakePhoto();

                if (photo == null) return;
                else await RecognizeAndVoiceHandwrittenText(photo);
            }
            catch (CameraException ex)
            {
                await SpeechSyntezer.VoiceResult(ex.Message);
            }
            catch (Exception)
            { 
                 await Navigation.PushAsync(new SomethingWentWrongPage());
            }
            finally
            {
                TakePhotoButton.IsEnabled = true;
            }
        }

        /// <summary>
        /// Выбрать фото из галереи, распознать рукописный текст и озвучить результат
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void GetPhotoButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                GetPhotoButton.IsEnabled = false;
                var photo = await CameraActions.GetPhoto();

                if (photo == null) return;
                else await RecognizeAndVoiceHandwrittenText(photo);
            }
            catch (CameraException ex)
            {
                await SpeechSyntezer.VoiceResult(ex.Message);
            }
            catch (Exception)
            {
                await Navigation.PushAsync(new SomethingWentWrongPage());
            }
            finally
            {
                GetPhotoButton.IsEnabled = true;
            }
        }

        /// <summary>
        /// Распознать рукописный текст и озвучить
        /// </summary>
        /// <param name="photo"></param>
        /// <returns></returns>
        private async Task RecognizeAndVoiceHandwrittenText(MediaFile photo)
        {
            BusyIndicator.IsVisible = true;
            BusyIndicator.IsBusy = true;

            this.BackgroundImageSource = ImageSource.FromStream(() =>
            {
                return photo.GetStreamWithImageRotatedForExternalStorage();
            });
            try
            {
                detectedText = await TextDetector.ReadHandwrittenText(photo.Path);

                BusyIndicator.IsVisible = false;
                BusyIndicator.IsBusy = false;

                await SpeechSyntezer.VoiceResultInEnglish(detectedText);
            }
            catch(TextDetectorException ex)
            {
                await SpeechSyntezer.VoiceResultInEnglish(ex.Message);
            }
            finally
            {
               BusyIndicator.IsVisible = false;
               BusyIndicator.IsBusy = false;
            }
        }

        /// <summary>
        /// Запускает голосовое управление
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RecordAndAnalyze(object sender, EventArgs e)
        {
            try
            {
                MainGrid.IsEnabled = false;
                Vibration.Vibrate();
                BottomPanel.BackgroundColor = Color.FromHex("#C00000");

                if (await AudioRecording.CheckAudioPermissions())
                {
                    await AudioRecording.RecordAudio();
                    await AnalizeCommandHandwritten();
                }
            }
            catch (Exception)
            {
                await Navigation.PushAsync(new SomethingWentWrongPage());
            }
            finally
            {
                MainGrid.IsEnabled = true;
                BottomPanel.BackgroundColor = Color.Black;
            }
        }

        /// <summary>
        /// Выполняет задачу заданную голосом на странице рукописного текста
        /// </summary>
        /// <param name="cameraCommand"></param>
        /// <returns></returns>
        public async Task DoCommandsActionOnHandwritten(string cameraCommand)
        {
            
            if (cameraCommand.Length < 3)
            {
                await SpeechSyntezer.VoiceResult("Такой команды не существует");
            }
            else
            {
                if (cameraCommand.Contains("камер"))
                {
                    var photo = await CameraActions.TakePhoto();
                    if (photo == null) return;
                    else await RecognizeAndVoiceHandwrittenText(photo);
                    return;
                }
                if (cameraCommand.Contains("гал"))
                {
                    MediaFile photo = await CameraActions.GetPhoto();
                    if (photo == null) return;
                    else await RecognizeAndVoiceHandwrittenText(photo);
                    return;
                }
                if (cameraCommand.Contains("наз"))
                {
                    await Navigation.PopToRootAsync();
                    return;
                }
                if (cameraCommand.Contains("повтор"))
                {
                    if (detectedText != null)
                    {
                        SpeechSyntezer.CancelSpeech();
                        await SpeechSyntezer.VoiceResultInEnglish(detectedText);
                    }
                    return;
                }
                else
                {
                    await SpeechSyntezer.VoiceResult("Такой команды не существует");
                    return;
                }
            }
            
        }

        /// <summary>
        /// Повторить озвучку результатов распознавания
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RepeatButton_Clicked(object sender, EventArgs e)
        {
            if (detectedText != null)
            {
                SpeechSyntezer.CancelEnglishSpeech();
                await SpeechSyntezer.VoiceResultInEnglish(detectedText);
            }
        }

        /// <summary>
        /// Отменяет озвучку при выходе со страницы
        /// </summary>
        protected override void OnDisappearing()
        {
            SpeechSyntezer.CancelEnglishSpeech();
        }

        

        #endregion
    }
}