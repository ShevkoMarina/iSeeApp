using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace MyApp.ViewModels.ErrorAndEmpty
{
    [Preserve(AllMembers = true)]
    public class SomethingWentWrongPageViewModel : BaseViewModel
    {
        #region Fields

        private string imagePath;

        private string header;

        private string content;

        #endregion

        #region Constructor

        public SomethingWentWrongPageViewModel()
        {
            this.ImagePath = "Error.png";
            this.Header = "Ошибка";
            this.Content = "Что-то пошло не так. Вернитесь на главную страницу";
        }

        #endregion

        #region Commands

        public ICommand GoBackCommand { get; set; }

        #endregion

        #region Properties
        public string ImagePath
        {
            get
            {
                return this.imagePath;
            }

            set
            {
                this.imagePath = value;
                this.NotifyPropertyChanged();
            }
        }

        public string Header
        {
            get
            {
                return this.header;
            }

            set
            {
                this.header = value;
                this.NotifyPropertyChanged();
            }
        }

        public string Content
        {
            get
            {
                return this.content;
            }

            set
            {
                this.content = value;
                this.NotifyPropertyChanged();
            }
        }

        #endregion

    }
}
