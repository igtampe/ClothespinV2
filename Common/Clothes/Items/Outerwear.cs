using Clothespin2.Common.Clothes.Sizes;

namespace Clothespin2.Common.Clothes.Items {

    /// <summary>Holds something that goes over a shirt (Like a Jacket, Coat, or Sweater)</summary>
    public class Outerwear : Washable, DistinguishableSizable {

        public override int Type => 2;

        /// <summary>Distinguisher of the size of this overshirt item</summary>
        public SizeDistinguisher Distinguisher { get; set; }

        /// <summary>Size of this overshirt item (probably a letter!)</summary>
        public string Size { get; set; } = "";

        /// <summary>Region of the size of this overshirt</summary>
        public string Region { get; set; } = "";

    }
}
