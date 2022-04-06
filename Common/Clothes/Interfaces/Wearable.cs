using System.Text.Json.Serialization;

namespace Clothespin2.Common.Clothes {

    /// <summary>Abstract class of a wearable item</summary>
    public abstract class Wearable : Identifiable, Nameable, Describable, Depictable {

        /// <summary>Type of this Wearable</summary>
        public int Type { get; set; } = 0;

        /// <summary>Name of this Wearable</summary>
        public string Name { get; set; } = "";

        /// <summary>Description of this wearable</summary>
        public string Description { get; set; } = "";

        /// <summary>URL to the image</summary>
        public string ImageURL { get; set; } = "";

        /// <summary>Color of this wearable item</summary>
        public string Color { get; set; } = "";

        /// <summary>Person this garment belongs to</summary>
        [JsonIgnore]
        public User? Owner { get; set; }

        /// <summary>List of Outfits this Wearable is in</summary>
        [JsonIgnore]
        public List<Outfit> Outfits { get; set; } = new();

        /// <summary>Indicates weather this Wearable is deleted or not</summary>
        public bool Deleted { get; set; } = false;
        
    }
}
