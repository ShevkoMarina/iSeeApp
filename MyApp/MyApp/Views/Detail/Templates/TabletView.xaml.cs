using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace MyApp.Views.Detail.Templates
{
    /// <summary>
    /// The Default view for detail page
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabletView
    {
        public TabletView()
        {
            InitializeComponent();
            this.ProductImage.Source = App.BaseImageUrl + "ReviewShoe.png";
        }
    }
}