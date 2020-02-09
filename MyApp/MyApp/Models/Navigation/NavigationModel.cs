using System.Runtime.Serialization;
using Xamarin.Forms.Internals;

namespace MyApp.Models.Navigation
{
    /// <summary>
    /// Model for the Navigation List and Tile with Cards page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DataContract]
    public class NavigationModel
    {
        #region Field

        private string itemImage;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of an item.
        /// </summary>
        [DataMember(Name = "itemName")]
        public string ItemName { get; set; }

        /// <summary>
        /// Gets or sets the description of an item.
        /// </summary>
        [DataMember(Name = "itemDescription")]
        public string ItemDescription { get; set; }

        /// <summary>
        /// Gets or sets the image of an item.
        /// </summary>
        [DataMember(Name = "itemImage")]
        public string ItemImage
        {
            get
            {
                return 
                    this.itemImage;
            }
            set
            {
                this.itemImage = value;
            }
        }

        #endregion
    }
}