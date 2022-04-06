using Clothespin2.Common.Clothes.Sizes;

namespace Clothespin2.Common.Clothes.Items {

    /// <summary>Holds a shirt</summary>
    public class Shirt : Washable, DistinguishableSizable {

        public override int Type => 0;

        /// <summary>Distinguisher for the size of this shirt</summary>
        public SizeDistinguisher Distinguisher { get; set; } = SizeDistinguisher.UNIVERSAL;

        /// <summary>Size of this shirt (Either a letter if its a normal shirt, or a pair of three numbers (Neck, Chest, Sleeve length) for dress shirts in inches if in the US)</summary>
        public string Size { get; set; } = "";

        /// <summary>Region of this size</summary>
        public string Region { get; set; } = "";
    }
}
