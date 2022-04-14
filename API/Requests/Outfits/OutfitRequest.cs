namespace Clothespin2.API.Requests {

    /// <summary>Abstract Outfit Request that has all fields</summary>
    public class OutfitRequest {

        /// <summary>ID of the shrit the requested outfits must have</summary>
        public Guid? ShirtID { get; set; } = null;

        /// <summary>ID of the pants the requested outfits must have</summary>
        public Guid? PantsID { get; set; } = null;

        /// <summary>ID of the Dress the requested outfits must have</summary>
        public Guid? DressID { get; set; } = null;

        /// <summary>ID of the shoes the requested outfits must have</summary>
        public Guid? ShoesID { get; set; } = null;

        /// <summary>IDs of the Outerwear the requested outfits must have</summary>
        public Guid[]? OuterwearIDs { get; set; } = null;

    }
}
