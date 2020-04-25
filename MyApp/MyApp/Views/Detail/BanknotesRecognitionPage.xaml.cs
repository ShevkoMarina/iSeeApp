using MyApp.RecognitionClasses;
using Plugin.Media.Abstractions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyApp.RecognitionClasses.CameraClass;
using System.Threading.Tasks;

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

        public async Task VoiceCommand(string cameraCommand)
        {
            switch(cameraCommand)
            {
                case "камера":
                    {
                        var photo = await CameraActions.TakePhoto();
                        if (photo == null) return;
                        else RecognizeAndVoiceBacknote(photo);
                        break;
                    }
                case "галерея":
                    {
                        MediaFile photo = await CameraActions.GetPhoto();
                        if (photo == null) return;
                        else RecognizeAndVoiceBacknote(photo);
                        break;
                    }
            }
        }

        private async void TakePhotoButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var photo = await CameraActions.TakePhoto();

                if (photo == null) return;
                else RecognizeAndVoiceBacknote(photo);
      
            }
            catch (BanknotesDetectionException)
            {
                await DisplayAlert("Error", "Banknote wasn't recognised", "OK");
            }
            catch (CameraException ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                //await Navigation.PushAsync(new SomethingWentWrongPage());
            }
        }

        private async void GetPhotoButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                MediaFile photo = await CameraActions.GetPhoto();

                if (photo == null) return;
                else RecognizeAndVoiceBacknote(photo);
                         
            }
            catch (CameraException ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                //await Navigation.PushAsync(new SomethingWentWrongPage());
            }
        }

        private async void RecognizeAndVoiceBacknote(MediaFile photo)
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

                await TextSyntezer.VoiceResult(detectedBanknote + "рублей");
            }
            catch (BanknotesDetectionException)
            {
                await TextSyntezer.VoiceResult("Банкнота не распознана. Попробуйте другое фото");
            }
            finally
            {
                BusyIndicator.IsVisible = false;
                BusyIndicator.IsBusy = false;
                this.BackgroundImageSource = "UploadPhoto.png";
            }
        }
        #endregion
    }
}