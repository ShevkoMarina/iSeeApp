using System;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using MyApp.RecognitionClasses;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using MyApp.RecognitionClasses.CameraClass;
using System.Threading.Tasks;

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

        public async Task VoiceCommand(string cameraCommand)
        {
            switch (cameraCommand)
            {
                case "камера":
                    {
                        var photo = await CameraActions.TakePhoto();
                        if (photo == null) return;
                        else RecognizeAndVoicePrintedText(photo);
                        break;
                    }
                case "галерея":
                    {
                        MediaFile photo = await CameraActions.GetPhoto();
                        if (photo == null) return;
                        else RecognizeAndVoicePrintedText(photo);
                        break;
                    }
            }
        }

        private async void TakePhotoButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                TakePhotoButton.IsEnabled = false;
                var photo = await CameraActions.TakePhoto();

                if (photo == null) return;
                else RecognizeAndVoicePrintedText(photo);
            }         
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
               // await Navigation.PushAsync(new SomethingWentWrongPage());
            }
            finally
            {
                TakePhotoButton.IsEnabled = true; 
            }
        }

        private async void GetPhotoButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                GetPhotoButton.IsEnabled = false;
                var photo = await CameraActions.GetPhoto();

                if (photo == null) return;
                else RecognizeAndVoicePrintedText(photo);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                // await Navigation.PushAsync(new SomethingWentWrongPage());
            }
            finally
            {
                GetPhotoButton.IsEnabled = true;
            }
        }

        private async void RecognizeAndVoicePrintedText(MediaFile photo)
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

                await TextSyntezer.VoiceResult(detectedText);
            }          
            catch (TextDetectorException)
            {
                await TextSyntezer.VoiceResult("Ничего не распознано. Попробуйте другое фото");
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