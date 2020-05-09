using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;

namespace MyApp.RecognitionClasses.CameraClass
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
                        DefaultCamera = CameraDevice.Rear,
                        PhotoSize = PhotoSize.Small,
                        SaveToAlbum = false,
                        Directory = "iSeeApp",
                        Name = $"{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.jpg",
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
                    MediaFile photo = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                    {
                        PhotoSize = PhotoSize.Small
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

   