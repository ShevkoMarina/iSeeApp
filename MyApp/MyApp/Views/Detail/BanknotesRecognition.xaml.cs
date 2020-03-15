using MyApp.Classes;
using MyApp.Views.ErrorAndEmpty;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Syncfusion.SfBusyIndicator.XForms;


namespace MyApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BanknotesRecognition : ContentPage
    {
        public BanknotesRecognition()
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
                            PhotoSize = PhotoSize.Small,
                            SaveToAlbum = true,
                            Directory = "MarinaApp",
                            Name = $"{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.jpg"

                        }); ;
                      
                        if (photo == null)
                            return;

                        BusyIndicator.IsVisible = true;
                        BusyIndicator.IsBusy = true;
                        this.BackgroundImageSource = ImageSource.FromStream(() =>
                        {
                            return photo.GetStreamWithImageRotatedForExternalStorage();
                        });
                        await BanknotesDetector.MakePredictionRequest(photo.Path);
                        BusyIndicator.IsVisible = false;
                        BusyIndicator.IsBusy = false;

                        await TextSyntezer.SpeakResult(BanknotesDetector.DetectedBanknote + "рублей");
                       
                    }
                }
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
                        MediaFile photo = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                        {
                            PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small
                        });
                    
                        if (photo == null)
                            return;

                        BusyIndicator.IsVisible = true;
                        BusyIndicator.IsBusy = true;
                        this.BackgroundImageSource = ImageSource.FromStream(() =>
                        {
                            return photo.GetStreamWithImageRotatedForExternalStorage();
                        });

                        await BanknotesDetector.MakePredictionRequest(photo.Path);
                        BusyIndicator.IsVisible = false;
                        BusyIndicator.IsBusy = false;

                        await TextSyntezer.SpeakResult(BanknotesDetector.DetectedBanknote + "рублей");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                //await Navigation.PushAsync(new SomethingWentWrongPage());
            }
        }
    }
}