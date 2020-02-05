using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace MyApp.Views.Detail
{
    /// <summary>
    /// Page to show the feedback in detail.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeedbackDetailPage
    {
        public FeedbackDetailPage()
        {
            InitializeComponent();
        }
    }
}