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

        public async Task VoiceCommand(string cameraCommand)
        {
            switch (cameraCommand)
            {
                case "камера":
                    {
                        var photo = await CameraActions.TakePhoto();
                        if (photo == null) return;
                        else RecognizeAndVoiceHandwrittenText(photo);
                        break;
                    }
                case "галерея":
                    {
                        MediaFile photo = await CameraActions.GetPhoto();
                        if (photo == null) return;
                        else RecognizeAndVoiceHandwrittenText(photo);
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
                else RecognizeAndVoiceHandwrittenText(photo);
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
                else RecognizeAndVoiceHandwrittenText(photo);
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

        private async void RecognizeAndVoiceHandwrittenText(MediaFile photo)
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

                await TextSyntezer.VoiceResultInEnglish(detectedText);
            }
            catch(TextDetectorException ex)
            {
                await TextSyntezer.VoiceResultInEnglish(ex.Message);
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