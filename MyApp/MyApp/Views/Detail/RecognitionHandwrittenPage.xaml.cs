using System;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using MyApp.RecognitionClasses;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using MyApp.Views.ErrorAndEmpty;
using MyApp.RecognitionClasses.CameraClass;

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

        private async void TakePhotoButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var photo = await CameraActions.TakePhoto();

                if (photo == null) return;
                else RecognizeAndVoiceHandwrittenText(photo);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                // await Navigation.PushAsync(new SomethingWentWrongPage());
            }
        }

        private async void GetPhotoButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var photo = await CameraActions.GetPhoto();

                if (photo == null) return;
                else RecognizeAndVoiceHandwrittenText(photo);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                // await Navigation.PushAsync(new SomethingWentWrongPage());
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

                await TextSyntezer.SpeakResultInEnglish(detectedText);
            }
            catch(TextDetectorException ex)
            {
                await TextSyntezer.SpeakResultInEnglish(ex.Message);
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