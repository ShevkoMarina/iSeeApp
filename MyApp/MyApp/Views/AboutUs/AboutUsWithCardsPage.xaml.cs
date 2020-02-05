using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace MyApp.Views.AboutUs
{
    /// <summary>
    /// About us with cards page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutUsWithCardsPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyApp.Views.AboutUs.AboutUsWithCardsPage"/> class.
        /// </summary>
        public AboutUsWithCardsPage()
        {
            InitializeComponent();
        }
    }
}