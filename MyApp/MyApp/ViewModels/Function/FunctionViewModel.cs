using MyApp.RecognitionClasses.CameraClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace MyApp.ViewModels.Function
{

    [Preserve(AllMembers = true)]

    public class FunctionViewModel : BaseViewModel

    {

        #region Constrctor
        public FunctionViewModel()
        {

            this.TakePhotoCommand = new Command(this.OnTakePhotoButtonClicked);
            this.GetPhotoCommand = new Command(this.OnGetPhotoButtonClicked);
        }
        #endregion

        #region Properties

        public Command TakePhotoCommand { get; private set; }

        public Command GetPhotoCommand { get; private set; }

        #endregion


        #region Methods
        private async void OnTakePhotoButtonClicked(object obj)
        {
            await CameraActions.TakePhoto();          
        }

        private async void OnGetPhotoButtonClicked(object obj)
        {
            await CameraActions.GetPhoto();
        }
        
        private async void OnStartRecognitionFunction(object obj)
        {

        }

        #endregion

    }
}
