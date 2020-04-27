using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MyApp.Models.Hints;
using MyApp.RecognitionClasses;
using MyApp.Views.Hints;
using Syncfusion.SfRotator.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace MyApp.ViewModels.Hints
{
    /// <summary>
    /// ViewModel for on-boarding gradient page with animation.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class OnBoardingAnimationViewModel : BaseViewModel
    {
        #region Fields

        private ObservableCollection<Boarding> boardings;

        private string nextButtonText = "NEXT";

        private bool isSkipButtonVisible = true;

        private int selectedIndex;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="OnBoardingAnimationViewModel" /> class.
        /// </summary>
        public OnBoardingAnimationViewModel()
        {
            this.SkipCommand = new Command(this.Skip);
            this.NextCommand = new Command(this.Next);
            this.Boardings = new ObservableCollection<Boarding>
            {
                new Boarding()
                {
                    ImagePath = "ReSchedule.png",
                    Header = "Главное меню",
                    Content = "Выберите в главном меню нужную функцию распознавания",
                    RotatorItem = new WalkthroughItemPage()
                },
                new Boarding()
                {
                    ImagePath = "ViewMode.png",
                    Header = "Выбор фото",
                    Content = "Сделайте фотографию на камеру или загрузите из галереи",
                    RotatorItem = new WalkthroughItemPage()
                },
                new Boarding()
                {
                    ImagePath = "TimeZone.png",
                    Header = "Озвучка",
                    Content = "Подождите, когда приложение озвучит текст на фото",
                    RotatorItem = new WalkthroughItemPage()
                },
                  new Boarding()
                {
                    ImagePath = "TimeZone.png",
                    Header = "Голосовое управление",
                    Content = "Вы можете управлять приложением голосом используя команды",
                    RotatorItem = new WalkthroughItemPage()
                },
                     new Boarding()
                {
                    ImagePath = "TimeZone.png",
                    Header = "Список команд",
                    Content = "Деньги, Печатный, Рукописный, Камера, Галерея, Настройки, Помощь",
                    RotatorItem = new WalkthroughItemPage()
                }
            };

            // Set bindingcontext to content view.
            foreach (var boarding in this.Boardings)
            {
                boarding.RotatorItem.BindingContext = boarding;
            }
        }

        #endregion

        #region Properties

        public ObservableCollection<Boarding> Boardings
        {
            get
            {
                return this.boardings;
            }

            set
            {
                if (this.boardings == value)
                {
                    return;
                }

                this.boardings = value;
                this.NotifyPropertyChanged();
            }
        }

        public string NextButtonText
        {
            get
            {
                return this.nextButtonText;
            }

            set
            {
                if (this.nextButtonText == value)
                {
                    return;
                }

                this.nextButtonText = value;
                this.NotifyPropertyChanged();
            }
        }

        public bool IsSkipButtonVisible
        {
            get
            {
                return this.isSkipButtonVisible;
            }

            set
            {
                if (this.isSkipButtonVisible == value)
                {
                    return;
                }

                this.isSkipButtonVisible = value;
                this.NotifyPropertyChanged();
            }
        }

        public int SelectedIndex
        {
            get
            {
                return this.selectedIndex;
            }

            set
            {
                if (this.selectedIndex == value)
                {
                    return;
                }

                this.selectedIndex = value;
                this.NotifyPropertyChanged();
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Gets or sets the command that is executed when the Skip button is clicked.
        /// </summary>
        public ICommand SkipCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Done button is clicked.
        /// </summary>
        public ICommand NextCommand { get; set; }

        #endregion

        #region Methods

        private bool ValidateAndUpdateSelectedIndex(int itemCount)
        {
            if (this.SelectedIndex >= itemCount - 1)
            {
                return true;
            }

            this.SelectedIndex++;
            return false;
        }

        /// <summary>
        /// Invoked when the Skip button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void Skip(object obj)
        {
            this.MoveToNextPage();
        }

        /// <summary>
        /// Invoked when the Done button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void Next(object obj)
        {            
            var itemCount = (obj as SfRotator).ItemsSource.Count();

            switch(selectedIndex)
            {
                case 0:
                    TextSyntezer.VoiceResult("Сделайте фотографию на камеру или загрузите из галереи");
                    break;
                case 1:
                    TextSyntezer.VoiceResult("Подождите, когда приложение озвучит текст на фото");                
                    break;
                case 2:
                    TextSyntezer.VoiceResult("Вы можете управлять приложением голосом используя команды");
                    break;
                case 3:
                    TextSyntezer.VoiceResult("Cписок команд: Деньги, Печатный, Рукописный, Камера, Галерея, Настройки, Помощь");
                    break;
                default:
                    break;
            }
            if (this.ValidateAndUpdateSelectedIndex(itemCount))
            {
                this.MoveToNextPage();
            }        
        }

        private void MoveToNextPage()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        #endregion
    }
}
