using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyApp.Views.Templates
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HandwrittenCardTemplate : Grid
    {
        public HandwrittenCardTemplate()
        {
            InitializeComponent();
        }
    }
}