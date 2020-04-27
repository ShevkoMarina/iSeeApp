﻿using MyApp.RecognitionClasses;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace MyApp.Views.Hints
{
    /// <summary>
    /// Page to display on-boarding gradient with animation
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WalkthroughItemPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WalkthroughItemPage" /> class.
        /// </summary>
        public WalkthroughItemPage()
        {
            InitializeComponent();
        }
    }
}