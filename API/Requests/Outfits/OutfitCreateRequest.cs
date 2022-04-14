using Clothespin2.Common;

namespace Clothespin2.API.Requests {

    /// <summary>Request to create an outfit</summary>
    public class OutfitCreateRequest : OutfitRequest, Nameable, Describable, Depictable {

        /// <summary>Name of the outfit</summary>
        public string Name { get; set; } = "";

        /// <summary>Description of the outfit</summary>
        public string Description { get; set; } = "";

        /// <summary>ImageURL of the outfit</summary>
        public string ImageURL { get; set; } = "";

    }
}
