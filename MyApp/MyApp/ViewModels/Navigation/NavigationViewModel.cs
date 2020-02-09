using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Xamarin.Forms;
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

        private Command<object> itemTappedCommand;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="NavigationViewModel"/> class.
        /// </summary>
        public NavigationViewModel()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the command that will be executed when an item is selected.
        /// </summary>
        public Command<object> ItemTappedCommand
        {
            get
            {
                return this.itemTappedCommand ?? (this.itemTappedCommand = new Command<object>(this.NavigateToNextPage));
            }
        }

        /// <summary>
        /// Gets or sets a collection of values to be displayed in the Navigation list page.
        /// </summary>
        [DataMember(Name = "navigationList1")]
        public ObservableCollection<NavigationModel> NavigationList1 { get; set; }

        /// <summary>
        /// Gets or sets a collection of values to be displayed in the Navigation list page.
        /// </summary>
        [DataMember(Name = "navigationList2")]
        public ObservableCollection<NavigationModel> NavigationList2 { get; set; }

        /// <summary>
        /// Gets or sets a collection of values to be displayed in the Navigation list page.
        /// </summary>
        [DataMember(Name = "navigationList3")]
        public ObservableCollection<NavigationModel> NavigationList3 { get; set; }


        #endregion

        #region Methods

        /// <summary>
        /// Invoked when an item is selected from the navigation list.
        /// </summary>
        /// <param name="selectedItem">Selected item from the list view.</param>
        private async void NavigateToNextPage(object selectedItem)
        {
            
        }

        #endregion
    }
}