using MyApp.RecognitionClasses;
using Plugin.Media.Abstractions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyApp.RecognitionClasses.CameraClass;
using System.Threading.Tasks;
using MyApp.Views.ErrorAndEmpty;
using System.Runtime.CompilerServices;

namespace MyApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BanknotesRecognitionPage : ContentPage
    {

        public BanknotesRecognitionPage()
        {         
            InitializeComponent();
            
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
        /// Распознование и озвучка при голосовом управлении
        /// </summary>
        /// <param name="cameraCommand"></param>
        /// <returns></returns>
        public async Task VoiceCommand(string cameraCommand)
        {
            switch(cameraCommand.Substring(0,3))
            {
                case "кам":
                    {
                        var photo = await CameraActions.TakePhoto();
                        if (photo == null) return;
                        else await RecognizeAndVoiceBacknote(photo);
                        break;
                    }
                case "гал":
                    {
                        MediaFile photo = await CameraActions.GetPhoto();
                        if (photo == null) return;
                        else await RecognizeAndVoiceBacknote(photo);
                        break;
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
                else await RecognizeAndVoiceBacknote(photo);
      
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
                else await RecognizeAndVoiceBacknote(photo);                       
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
        /// Распознование банкноты и озвучка номинала
        /// </summary>
        /// <param name="photo"></param>
        /// <returns></returns>
        private async Task RecognizeAndVoiceBacknote(MediaFile photo)
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

                await SpeechSyntezer.VoiceResult(detectedBanknote + "рублей");
            }
            catch (BanknotesDetectionException)
            {           
                await SpeechSyntezer.VoiceResult("Банкнота не распознана");
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
                await SpeechSyntezer.VoiceResult(detectedBanknote + "рублей");
            }
        }
        #endregion
    }
}