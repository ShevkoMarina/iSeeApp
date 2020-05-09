using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Xamarin.Forms.Internals;
using NavigationModel = MyApp.Models.Navigation.NavigationModel;

namespace MyApp.ViewModels.Navigation
{
    /// <summary>
    /// ViewModel for the Navigation list with cards page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DataContract]
    public class NavigationViewModel
    {
        #region Fields

        private ObservableCollection<NavigationModel> functionsList;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="NavigationViewModel"/> class.
        /// </summary>
        public NavigationViewModel()
        {
            functionsList =  new ObservableCollection<NavigationModel>()
            {
                new NavigationModel()
                {
                    FunctionName = "Печатный текст",
                    FunctionDescription = "Ценники, этикетки в магазине, меню",
                    FunctionImage = "Printed.png"
                },
                new NavigationModel()
                {
                    FunctionName = "Банкноты",
                    FunctionDescription = "Распознавание номиналов банкнот",
                    FunctionImage = "Banknotes2.png"
                },
                new NavigationModel()
                {
                    FunctionName = "Рукописный текст",
                    FunctionDescription = "Рукописные записи, заметки",
                    FunctionImage = "Handwritten.png"
                },
            };
        }

        #endregion

        #region Properties

        public ObservableCollection<NavigationModel> FunctionsList { get => functionsList; set => functionsList = value; }

        #endregion
    }
}