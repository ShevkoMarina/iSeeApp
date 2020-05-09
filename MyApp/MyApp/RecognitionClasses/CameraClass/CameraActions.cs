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

       /// <summary>
       /// Сделать фото с помощью камеры девайса и сохранить в альбом приложения в галереи
       /// </summary>
       /// <returns></returns>
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
                        SaveToAlbum = true,
                        Directory = "iSeeApp",
                        Name = $"{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.jpg",
                    }); ;

                    return photo;
                }
                else
                {
                    throw new CameraException("Камера не поддерживается");
                }
            }
        }

        /// <summary>
        /// Выбрать фото из галлереи
        /// </summary>
        /// <returns></returns>
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
                    throw new CameraException("Загрузка фото из галереи не поддерживается");
                }
            }
        }
        #endregion
    }
}

   