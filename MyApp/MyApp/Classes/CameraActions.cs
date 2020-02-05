using System;
using System.Threading.Tasks;
using Plugin.Permissions.Abstractions;
using Plugin.Permissions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Collections.Generic;
using System.Text;
using MyApp.Classes;
namespace MyApp
{
    public class CameraActions
    {
        public static async Task TakePhoto()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);

                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                    {
                       // await DisplayAlert("Need camera", "I need permition", "OK");
                    }
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
                        MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                        {
                            SaveToAlbum = true,
                            Directory = "MarinaApp",
                            Name = $"{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.jpg"
                        });

                        if (file == null)
                            return;
                        await TextDetector.ReadTextInEnglish(file.Path);
                        
                        await TextSyntezer.SpeakResult(TextDetector.DetectedText);

                    }
                }
            }
            catch (Exception ex)
            {
               // await DisplayAlert("Error", ex.Message, "OK");
            }
        }
        public static async Task GetPhoto()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                    {
                        //await DisplayAlert("Need camera", "I need permition", "OK");
                    }
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
                        await TextDetector.ReadTextInEnglish(photo.Path);
                        await TextSyntezer.SpeakResult(TextDetector.DetectedText);

                        //img.Source = ImageSource.FromFile(photo.Path);
                    }
                }
            }
            catch (Exception ex)
            {
                
                //await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
