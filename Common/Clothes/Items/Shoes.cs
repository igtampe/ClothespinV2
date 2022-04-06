using Clothespin2.Common.Clothes.Sizes;

namespace Clothespin2.Common.Clothes.Items {

    /// <summary>Holds a pair of shoes</summary>
    public class Shoes : Wearable, DistinguishableSizable {

        /// <summary>Distinguisher for these shoes</summary>
        public SizeDistinguisher Distinguisher { get; set; } = SizeDistinguisher.UNIVERSAL;

        /// <summary>Size of this Shoe</summary>
        public string Size { get; set; } = "";

        /// <summary>Region of the size of this overshirt</summary>
        public string Region { get; set; } = "";

    }
}
