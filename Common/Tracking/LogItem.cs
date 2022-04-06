using Clothespin2.Common.Clothes;
using Clothespin2.Common.Clothes.Items;

namespace Clothespin2.Common.Tracking {

    /// <summary>A log of something at a specific time</summary>
    public abstract class LogItem:Identifiable {

        /// <summary>User that logged this logitem</summary>
        public User? Owner { get; set; }

        /// <summary>Day this outfit was worn</summary>
        public DateTime? Date { get; set; }

        /// <summary>Note for this log item</summary>
        public string Note { get; set; } = "";

        /// <summary>Optional outfit of this logitem</summary>
        public Outfit? Outfit { get; set; }

        /// <summary>Shirt worn in this logitem</summary>
        public Shirt? Shirt { get; set; }

        /// <summary>Pants worn in this logitem</summary>
        public Pants? Pants { get; set; }

        /// <summary>Dress worn on this logitem</summary>
        public Dress? Dress { get; set; }

        /// <summary>Outerwear Layers worn on this logitem</summary>
        public List<Outerwear> OuterwearLayers { get; set; } = new();

        /// <summary>Shoes worn on this logitem</summary>
        public Shoes? Shoes { get; set; }
    }
}
