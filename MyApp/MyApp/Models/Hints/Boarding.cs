using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace MyApp.Models.Hints
{
    [Preserve(AllMembers = true)]
    public class Boarding
    {
        #region Properties

        public string ImagePath { get; set; }

        public string Header { get; set; }

        public string Content { get; set; }

        public View RotatorItem { get; set; }

        #endregion
    }
}
