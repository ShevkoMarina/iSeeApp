using System;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using MyApp.RecognitionClasses;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using MyApp.Views.ErrorAndEmpty;
using MyApp.RecognitionClasses.CameraClass;
using System.Threading.Tasks;

namespace MyApp.Views.Detail
{
   
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecognitionHandwrittenPage
    {
        public RecognitionHandwrittenPage()
        {
            InitializeComponent();
        }
        private string detectedText;

        #region Methods

        /// <summary>
        /// Распознать рукописный текст с помощью голосового управления
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
                        else await RecognizeAndVoiceHandwrittenText(photo);
                        break;
                    }
                case "галерея":
                    {
                        MediaFile photo = await CameraActions.GetPhoto();
                        if (photo == null) return;
                        else await RecognizeAndVoiceHandwrittenText(photo);
                        break;
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
            catch (Exception ex)
            {
                await DisplayAlert("ddd", ex.Message, "OK");
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