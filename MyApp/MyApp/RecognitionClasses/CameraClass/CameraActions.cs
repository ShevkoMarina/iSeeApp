using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using Xamarin.Forms;
using MyApp.Views;
using System.Threading.Tasks;

namespace MyApp.RecognitionClasses.CameraClass
// На каком языке приложение?
{
    public static class CameraActions
    {
        #region Methods
        public static async Task<MediaFile> TakePhoto()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);

            if (status != PermissionStatus.Granted)
            {
                await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                return null;
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
                        Name = $"{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.jpg",
                        DefaultCamera = CameraDevice.Rear,

                    }); ;

                    return photo;
                }
                else
                {
                    throw new CameraException("Camera is not supported");
                }
            }
        }

        public static async Task<MediaFile> GetPhoto()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);

            if (status != PermissionStatus.Granted)
            {
                await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                return null;
            }
            else
            {
                if (CrossMedia.Current.IsPickPhotoSupported)
                {
                    MediaFile photo = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                    {
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small
                    });

                    return photo;
                }
                else
                {
                    throw new CameraException("Picking photo is not supported");
                }
            }
        }
            #endregion
    }
}

   