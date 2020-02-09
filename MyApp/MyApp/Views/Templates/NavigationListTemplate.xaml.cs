using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyApp.Views.Detail;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace MyApp.Views.Templates
{
    /// <summary>
    /// Navigation list template.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigationListTemplate : Grid
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationListTemplate"/> class.
        /// </summary>
		public NavigationListTemplate()
        {
            InitializeComponent();  
        }
        
        private async void PrintedTextButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RecognitionPage());
        }
    }
}