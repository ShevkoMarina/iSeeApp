using System.Windows.Input;
using Xamarin.Forms.Internals;

namespace MyApp.ViewModels.ErrorAndEmpty
{
    /// <summary>
    /// ViewModel for no internet connection page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class NoInternetConnectionPageViewModel : BaseViewModel
    {
        #region Fields

        private string imagePath;

        private string header;

        private string content;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="NoInternetConnectionPageViewModel" /> class.
        /// </summary>
        public NoInternetConnectionPageViewModel()
        {
            this.ImagePath = "NoInternet.png";
            this.Header = "НЕТ ИНТЕРНЕТА";
            this.Content = "Проверьте свое подключение к сети Интернет";
           
        }

        #endregion

        #region Commands

        /// <summary>
        /// Gets or sets the command that is executed when the TryAgain button is clicked.
        /// </summary>
        public ICommand TryAgainCommand { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ImagePath.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the Header.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the Content.
        /// </summary>
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
