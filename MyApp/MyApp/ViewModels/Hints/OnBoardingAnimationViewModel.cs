using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using MyApp.Models.Hints;
using MyApp.RecognitionClasses;
using MyApp.Views.Hints;
using Syncfusion.SfRotator.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace MyApp.ViewModels.Hints
{

    [Preserve(AllMembers = true)]
    public class OnBoardingAnimationViewModel : BaseViewModel
    {
        #region Fields

        private ObservableCollection<Boarding> boardings;

        private string nextButtonText = "Далее";

        private bool isSkipButtonVisible = true;

        private int selectedIndex;

        #endregion

        #region Constructor

        public OnBoardingAnimationViewModel()
        {
            this.SkipCommand = new Command(this.Skip);
            this.NextCommand = new Command(this.Next);
            this.Boardings = new ObservableCollection<Boarding>
            {
                new Boarding()
                {
                    ImagePath = "MainForTips.png",
                    Header = "Главное меню",
                    Content = "Выберите в главном меню нужную функцию распознавания",
                    RotatorItem = new WalkthroughItemPage()
                },
                new Boarding()
                {
                    ImagePath = "DetailForTips.png",
                    Header = "Выбор фото",
                    Content = "Сделайте фотографию на камеру или загрузите из галереи",
                    RotatorItem = new WalkthroughItemPage()
                },
                new Boarding()
                {
                    ImagePath = "WaitVoicing.png",
                    Header = "Озвучка",
                    Content = "Подождите, когда приложение озвучит текст на фото",
                    RotatorItem = new WalkthroughItemPage()
                },
                new Boarding()
                {
                    ImagePath = "WaitVoicing.png",
                    Header = "Повтор",
                    Content = "Вы можете еще раз прослушать результат, нажав на кнопку повтора",
                    RotatorItem = new WalkthroughItemPage()
                },
                  new Boarding()
                {
                    ImagePath = "VoiceForTips.png",
                    Header = "Голосовое управление",
                    Content = "Вы можете управлять приложением голосом используя команды",
                    RotatorItem = new WalkthroughItemPage()
                },
                     new Boarding()
                {
                    ImagePath = "TipsForTips.png",
                    Header = "Команды",
                    Content = "Функция: печатный, деньги, рукописный, помощь\n\rФото: камера, галерея ",
                    RotatorItem = new WalkthroughItemPage()
                }
            };

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

        public ICommand SkipCommand { get; set; }

        public ICommand NextCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Проверяет и обноляет индекс просматриваемой страницы
        /// </summary>
        /// <param name="itemCount"></param>
        /// <returns></returns>
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
        /// Выход со страницы подсказок при нажатии на кнопку
        /// </summary>
        /// <param name="obj">The Object</param>
        private void Skip(object obj)
        {
            SpeechSyntezer.CancelSpeech();
            this.MoveToNextPage();
        }

        /// <summary>
        /// Переход на следующую страницу при нажатии на кнопку
        /// </summary>
        /// <param name="obj">The Object</param>
        private void Next(object obj)
        {
            SpeechSyntezer.CancelSpeech();
            var itemCount = (obj as SfRotator).ItemsSource.Count();

            switch(selectedIndex)
            {
                case 0:
                    SpeechSyntezer.VoiceResult("Сделайте фотографию на камеру или загрузите из галереи");
                    break;
                case 1:
                    SpeechSyntezer.VoiceResult("Подождите, когда приложение озвучит текст на фото");                
                    break;
                case 2:
                    SpeechSyntezer.VoiceResult("Вы можете еще раз прослушать результат, нажав на кнопку повтора");
                    break;
                case 3:
                    SpeechSyntezer.VoiceResult("Вы можете управлять приложением голосом используя команды");                     
                    break;
                case 4:
                    SpeechSyntezer.VoiceResult("Команда состоит из одной или двух частей. Первая: название функции. " +
                       "Печатный. Деньги. Рукописный. Помощь. Вторая часть это способ выбора фото.    камера. или. галерея.");
                    break;
                default:
                    break;
            }
            if (this.ValidateAndUpdateSelectedIndex(itemCount))
            {
              
                this.MoveToNextPage();
            }        
        }

        /// <summary>
        /// Выход со страницы подсказок
        /// </summary>
        private void MoveToNextPage()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        #endregion
    }
}
