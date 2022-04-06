using Clothespin2.Common.Clothes.Sizes;

namespace Clothespin2.Common.Clothes.Items {

    /// <summary>Holds a Dress</summary>
    public class Dress : Washable, DistinguishableSizable {
        public override int Type => 1;

        /// <summary>Distinguisher for the size of this Dress</summary>
        public SizeDistinguisher Distinguisher { get; set; } = SizeDistinguisher.UNIVERSAL;

        /// <summary>Size of this Dress (IDK how dresses are sized)</summary>
        public string Size { get; set; } = "";

        /// <summary>Region of this size</summary>
        public string Region { get; set; } = "";

    }
}
