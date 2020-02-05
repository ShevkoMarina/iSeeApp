using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MyApp.Controls.BorderlessEntry), typeof(MyApp.iOS.BorderlessEntryRenderer))]

namespace MyApp.iOS
{
    public class BorderlessEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (this.Control != null)
            {
                this.Control.BorderStyle = UITextBorderStyle.None;
            }
        }
    }
}