namespace Clothespin2.Common.Clothes {

    /// <summary>State of a Washable Wearable Item</summary>
    public enum WashState {

        /// <summary>A washable wearable that is clean and ready for use</summary>
        CLEAN = 0,

        /// <summary>A washable wearable that is semi-clean (used but still usable)</summary>
        SEMICLEAN = 1,

        /// <summary>A washable wearable that must be washed before use</summary>
        DIRTY = 2,

        /// <summary>A washable wearable that is currently in a washer or dryer, and is being cleaned</summary>
        WASHING = 3,

        /// <summary>A washable wearable that is currently clean, but must be stored (fold or hung)</summary>
        WASHED = 5,

    }

    /// <summary>A Wearable item that's washable</summary>
    public abstract class Washable : Wearable {

        /// <summary>Currently wash state that this washable wearable is in</summary>
        public WashState State { get; set; } = 0;

    }
}
