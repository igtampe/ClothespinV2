using Clothespin2.Common.Clothes.Items;

namespace Clothespin2.Common.Clothes {

    /// <summary>Holds an outfit (either a predetermined saved one, or a single use one)</summary>
    public class Outfit : Identifiable, Nameable, Describable, Depictable {

        /// <summary>Name of this outfit</summary>
        public string Name { get; set; } = "";

        /// <summary>Description of this outfit</summary>
        public string Description { get; set; } = "";

        /// <summary>Owner of this outfit</summary>
        public User? Owner { get; set; }

        /// <summary>Dress worn on this outfit </summary>
        public Dress? Dress { get; set; }

        /// <summary>Shirt worn on this outfit</summary>
        public Shirt? Shirt { get; set; }

        /// <summary>Pants worn on this outfit</summary>
        public Pants? Pants { get; set; }

        /// <summary>Shoes worn on this outfit</summary>
        public Shoes? Shoes { get; set; }

        /// <summary>List of outerwear items worn on this outfit</summary>
        public List<Outerwear> OuterwearLayers { get; set; } = new();

        /// <summary>Whether or not this Outfit has been deleted</summary>
        public bool Deleted { get; set; } = false;

        /// <summary>Image URL for this Outfit</summary>
        public string ImageURL { get; set; } = "";

        /// <summary>State of this Outfit</summary>
        /// <returns>Clean or Semiclean if the most dirty item has this state. Otherwise, Dirty</returns>
        public WashState OutfitState() { 
            
            List<WashState> StateList = new();
            StateList.Add(Shirt?.State ?? WashState.CLEAN);
            StateList.Add(Pants?.State ?? WashState.CLEAN);
            StateList.Add(Dress?.State ?? WashState.CLEAN);
            OuterwearLayers.ForEach(OS => StateList.Add(OS.State));
            StateList.Sort();
            WashState MAS = StateList.Last(); //This may actually be really overcomplicated pero zoop
            return MAS == WashState.CLEAN || MAS==WashState.SEMICLEAN ? MAS : WashState.DIRTY; 
            //I don't want to return MAS if it's anyhting besides dirty (Like washed) because while something might be washed, others may still be dirty.
            
        }
    }
}
