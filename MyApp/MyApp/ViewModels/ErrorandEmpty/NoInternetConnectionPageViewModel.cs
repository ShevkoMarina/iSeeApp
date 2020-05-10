using System.Windows.Input;
using Xamarin.Forms.Internals;

namespace MyApp.ViewModels.ErrorAndEmpty
{
    [Preserve(AllMembers = true)]
    public class NoInternetConnectionPageViewModel : BaseViewModel
    {
        #region Fields

        private string imagePath;

        private string header;

        private string content;

        #endregion

        #region Constructor

        public NoInternetConnectionPageViewModel()
        {
            this.ImagePath = "NoInternet.png";
            this.Header = "НЕТ ИНТЕРНЕТА";
            this.Content = "Проверьте свое подключение к сети Интернет";
           
        }

        #endregion


        public ICommand TryAgainCommand { get; set; }


        #region Properties

        public string ImagePath
        {
            get
            {
                return this.imagePath;
            }

            private set
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
