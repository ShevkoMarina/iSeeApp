using System;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using MyApp.RecognitionClasses;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using MyApp.RecognitionClasses.CameraClass;
using System.Threading.Tasks;
using MyApp.Views.ErrorAndEmpty;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Xamarin.Essentials;

namespace MyApp.Views.Detail
{

    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecognitionPrintedPage
    {      
        public RecognitionPrintedPage()
        {
            InitializeComponent();         
        }

        private string detectedText;

        #region Methods

        /// <summary>
        /// Распознать печатный текст с помощью голосового управления
        /// </summary>
        /// <param name="cameraCommand"></param>
        /// <returns></returns>
        public async Task VoiceCommand(string cameraCommand)
        {
            switch (cameraCommand)
            {
                case "камера":
                    {
                        var photo = await CameraActions.TakePhoto();
                        if (photo == null) return;
                        else await RecognizeAndVoicePrintedText(photo);
                        break;
                    }
                case "галерея":
                    {
                        MediaFile photo = await CameraActions.GetPhoto();
                        if (photo == null) return;
                        else await RecognizeAndVoicePrintedText(photo);
                        break;
                    }
            }
        }

        /// <summary>
        ///  Сделать фото, распознать печатный текст и озвучить результат
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
                else await RecognizeAndVoicePrintedText(photo);
            }
            catch (CameraException ex)
            {
                await SpeechSyntezer.VoiceResult(ex.Message);
            }
            
            catch (Exception ex)
            {
                await DisplayAlert("OK", ex.Message, "OK");
                await Navigation.PushAsync(new SomethingWentWrongPage());
            }
            finally
            {
                TakePhotoButton.IsEnabled = true; 
            }
        }

        /// <summary>
        ///  Выбрать фото из галереи, распознать печтаный текст и озвучить результат
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
                else await RecognizeAndVoicePrintedText(photo);
            }
            catch (CameraException ex)
            {
                await SpeechSyntezer.VoiceResult(ex.Message);
            }
            catch (Exception ex)
            {
                await DisplayAlert("OK", ex.Message, "OK");
                await Navigation.PushAsync(new SomethingWentWrongPage());
            }
            finally
            {
                GetPhotoButton.IsEnabled = true;
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
                Vibration.Vibrate(300);
                BottomPanel.BackgroundColor = Color.FromHex("#C00000");

                if (await AudioRecording.CheckAudioPermissions())
                {
                    await AudioRecording.RecordAudio();
                    await AnalizeCommandPrinted();
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
        /// Распознать печатный текст и озвучить
        /// </summary>
        /// <param name="photo"></param>
        /// <returns></returns>
        private async Task RecognizeAndVoicePrintedText(MediaFile photo)
        {
            try
            {
                BusyIndicator.IsVisible = true;
                BusyIndicator.IsBusy = true;

                this.BackgroundImageSource = ImageSource.FromStream(() =>
                {
                    return photo.GetStreamWithImageRotatedForExternalStorage();
                });
                detectedText = await TextDetector.ReadPrintedText(photo.Path);

                BusyIndicator.IsVisible = false;
                BusyIndicator.IsBusy = false;

                await SpeechSyntezer.VoiceResult(detectedText);
            }          
            catch (TextDetectorException)
            {
                await SpeechSyntezer.VoiceResult("Ничего не распознано. Попробуйте другое фото");
            }
            finally
            {
                BusyIndicator.IsVisible = false;
                BusyIndicator.IsBusy = false;
            }
        }

        /// <summary>
        /// Повторить озвучку результатов распознавания
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RepeatButtton_Clicked(object sender, EventArgs e)
        {
            if (detectedText != null)
            {
                SpeechSyntezer.CancelSpeech();
                await SpeechSyntezer.VoiceResult(detectedText);
            }
        }

        /// <summary>
        /// Отменяет озвучку при выходе со страницы
        /// </summary>
        protected override void OnDisappearing()
        {
            SpeechSyntezer.CancelSpeech();
        }

        /// <summary>
        /// Преобразует речь в текст и запускает выполнение команды
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private async Task AnalizeCommandPrinted()
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
                                await DoCommandsActionOnPrinted(processedText);
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
        /// Выполняет задачу заданную голосом на странице печатного текста
        /// </summary>
        /// <param name="cameraCommand"></param>
        /// <returns></returns>
        public async Task DoCommandsActionOnPrinted(string cameraCommand)
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
                    else await RecognizeAndVoicePrintedText(photo);
                    return;
                }
                if (cameraCommand.Contains("гал"))
                {
                    MediaFile photo = await CameraActions.GetPhoto();
                    if (photo == null) return;
                    else await RecognizeAndVoicePrintedText(photo);
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
                        await SpeechSyntezer.VoiceResult(detectedText);
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
        #endregion
    }
}