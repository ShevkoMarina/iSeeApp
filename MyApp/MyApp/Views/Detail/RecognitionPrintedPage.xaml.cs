using System;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using MyApp.Classes;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using MyApp.Views.ErrorAndEmpty;
using System.Drawing;

namespace MyApp.Views.Detail
{
   
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecognitionPrintedPage
    {
        // Обработать ошибку когда текста на фото не распознано
        public RecognitionPrintedPage()
        {
            InitializeComponent();
        }

        private async void TakePhotoButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);

                if (status != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                    if (results.ContainsKey(Permission.Camera))
                    {
                        status = results[Permission.Camera];
                    }
                }
                else
                {
                    if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
                    {
                        MediaFile photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                        {
                            SaveToAlbum = true,
                            Directory = "MarinaApp",
                            Name = $"{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.jpg"
                        });

                        if (photo == null)
                            return;
                        this.BackgroundImageSource = ImageSource.FromStream(() =>
                        {
                            return photo.GetStream();
                        });
                        //UserImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });

                        // await DisplayAlert("Info", Environment.GetFolderPath("\data\"), "OK"); ;
                        BusyIndicator.IsVisible = true;
                        BusyIndicator.IsBusy = true;
                        await TextDetector.ReadGoogle(photo.Path);

                        BusyIndicator.IsVisible = false;
                        BusyIndicator.IsBusy = false;

                        // await TextDetector.ReadPrintedText(photo.Path);
                        await DisplayAlert("Info", TextDetector.DetectedText, "OK");
                     
                       var locales = await TextSyntezer.SpeakResult(TextDetector.DetectedText);
                       

                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Info", ex.Message, "OK");
                await Navigation.PushAsync(new SomethingWentWrongPage());
            }
        }
        private async void GetPhotoButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                if (status != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                    if (results.ContainsKey(Permission.Camera))
                    {
                        status = results[Permission.Camera];
                    }
                }
                else
                {
                    if (CrossMedia.Current.IsPickPhotoSupported)
                    {
                        MediaFile photo = await CrossMedia.Current.PickPhotoAsync();
                        
                        if (photo == null)
                            return;
                      
                        this.BackgroundImageSource = ImageSource.FromStream(() =>
                        {
                            return photo.GetStreamWithImageRotatedForExternalStorage();
                        });
                           // UserImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
                           //await TextDetector.ReadPrintedText(photo.Path);


                        BusyIndicator.IsVisible = true;
                        BusyIndicator.IsBusy = true;
                        await TextDetector.ReadGoogle(photo.Path);

                        BusyIndicator.IsVisible = false;
                        BusyIndicator.IsBusy = false;

                        await DisplayAlert("Info", TextDetector.DetectedText, "OK");
                        await TextSyntezer.SpeakResult(TextDetector.DetectedText);
                        //this.BackgroundImageSource = "UploadPhoto.png";
                    }
                }
            }
            catch (Exception ex)
            {
                await Navigation.PushAsync(new SomethingWentWrongPage());
            }
        }
    }
}