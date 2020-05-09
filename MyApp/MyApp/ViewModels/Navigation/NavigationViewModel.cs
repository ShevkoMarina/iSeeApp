using System.Collections.ObjectModel;
using Xamarin.Forms.Internals;
using NavigationModel = MyApp.Models.Navigation.NavigationModel;

namespace MyApp.ViewModels.Navigation
{

    [Preserve(AllMembers = true)]
    public class NavigationViewModel
    {
        #region Fields

        private ObservableCollection<NavigationModel> functionsList;

        #endregion

        #region Constructor

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