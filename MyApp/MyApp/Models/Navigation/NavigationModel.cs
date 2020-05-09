using Xamarin.Forms.Internals;

namespace MyApp.Models.Navigation
{

    [Preserve(AllMembers = true)]
    public class NavigationModel
    {
        #region Field

        private string itemImage;

        #endregion

        #region Properties

        public string FunctionName { get; set; }

        public string FunctionDescription { get; set; }

        public string FunctionImage
        {
            get => this.itemImage;
            set => this.itemImage = value;
        }

        #endregion
    }
}