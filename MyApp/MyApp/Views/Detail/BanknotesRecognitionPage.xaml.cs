using MyApp.RecognitionClasses;
using Plugin.Media.Abstractions;
using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using MyApp.RecognitionClasses.CameraClass;
using System.Threading.Tasks;
using MyApp.Views.ErrorAndEmpty;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech;

namespace MyApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BanknotesRecognitionPage : ContentPage
    {

        public BanknotesRecognitionPage()
        {
            InitializeComponent();
            SpeechSyntezer.VoiceResult("Банкноты");
        }

        private string detectedBanknote;

        #region Methods

        /// <summary>
        /// Отменяет озвучку при выходе со страницы
        /// </summary>
        protected override void OnDisappearing()
        {
            SpeechSyntezer.CancelSpeech();
        }

        /// <summary>
        /// Выполняет задачу заданную голосом на странице распознавания печатного
        /// </summary>
        /// <param name="cameraCommand"></param>
        /// <returns></returns>
        public async Task DoCommandActionOnBanknote(string cameraCommand)
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
                    else await RecognizeAndVoiceBanknote(photo);
                    return;
                }
                if (cameraCommand.Contains("гал"))
                {
                    MediaFile photo = await CameraActions.GetPhoto();
                    if (photo == null) return;
                    else await RecognizeAndVoiceBanknote(photo);
                    return;
                }
                if (cameraCommand.Contains("наз"))
                {
                    await Navigation.PopToRootAsync();
                    return;
                }
                if (cameraCommand.Contains("повтор"))
                {
                    if (detectedBanknote != null)
                    {
                        SpeechSyntezer.CancelSpeech();
                        await SpeechSyntezer.VoiceResult(detectedBanknote);
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
        /// Фотографирование с камеры, распознование банкноты и озвучка номинала
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
                else await RecognizeAndVoiceBanknote(photo);
      
            }
            catch (CameraException ex)
            {
                await SpeechSyntezer.VoiceResult(ex.Message);
            }
            catch (BanknotesDetectionException ex)
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
        ///  Выбор фото из галереи, распознование банкноты и озвучка номинала
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void GetPhotoButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                GetPhotoButton.IsEnabled = false;
                MediaFile photo = await CameraActions.GetPhoto();

                if (photo == null) return;
                else await RecognizeAndVoiceBanknote(photo);                       
            }
            catch (CameraException ex)
            {
                await SpeechSyntezer.VoiceResult(ex.Message);
            }
            catch (BanknotesDetectionException ex)
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
        /// Распознование банкноты и озвучка номинала
        /// </summary>
        /// <param name="photo"></param>
        /// <returns></returns>
        private async Task RecognizeAndVoiceBanknote(MediaFile photo)
        {
            try
            {
                BusyIndicator.IsVisible = true;
                BusyIndicator.IsBusy = true;

                this.BackgroundImageSource = ImageSource.FromStream(() =>
                {
                    return photo.GetStreamWithImageRotatedForExternalStorage();
                });
                detectedBanknote = await BanknotesDetector.MakeBanknotesDetectionRequest(photo.Path);

                BusyIndicator.IsVisible = false;
                BusyIndicator.IsBusy = false;

                await SpeechSyntezer.VoiceResult(detectedBanknote);
            }
            finally
            {
                BusyIndicator.IsVisible = false;
                BusyIndicator.IsBusy = false;
            }
        }

        /// <summary>
        /// Повторно озвучить результат распознавания
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RepeatButton_Clicked(object sender, EventArgs e)
        {
            if (detectedBanknote != null)
            {
                SpeechSyntezer.CancelSpeech();
                await SpeechSyntezer.VoiceResult(detectedBanknote);
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
                    await AnalizeAudioCommandBanknote();
                }
            }
            catch(Exception)
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
        /// Преобразует речь в текст и запускает выполнение команды
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private async Task AnalizeAudioCommandBanknote()
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
                                await DoCommandActionOnBanknote(processedText);
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
        #endregion
    }
}