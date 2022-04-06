using Clothespin2.Common.Clothes.Sizes;

namespace Clothespin2.Common.Clothes.Items {
    /// <summary>Holds a pair of pants</summary>
    public class Pants : Washable, DistinguishableSizable {
        
        public override int Type => 3;

        /// <summary>Distinguisher of the size of these pants</summary>
        public SizeDistinguisher Distinguisher { get; set; } = SizeDistinguisher.UNIVERSAL;

        /// <summary>Size of this pants pair (Usually a number, except with male pants that are two numbers (Waist circumference, and pant length in inches if in the US)</summary>
        public string Size { get; set; } = "";

        /// <summary>Region of this pair of pants' size</summary>
        public string Region { get; set; } = "";
    }
}
