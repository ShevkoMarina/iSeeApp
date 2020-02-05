﻿using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace MyApp.Models.Hints
{
    /// <summary>
    /// Model for OnBoarding
    /// </summary>
    [Preserve(AllMembers = true)]
    public class Boarding
    {
        #region Properties

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        public View RotatorItem { get; set; }

        #endregion
    }
}
