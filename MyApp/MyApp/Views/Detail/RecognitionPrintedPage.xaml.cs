using System;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using MyApp.RecognitionClasses;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using MyApp.RecognitionClasses.CameraClass;
using System.Threading.Tasks;
using MyApp.Views.ErrorAndEmpty;

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

        #endregion
    }
}