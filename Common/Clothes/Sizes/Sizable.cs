namespace Clothespin2.Common.Clothes.Sizes {

    /// <summary>A Wearable that is also sizable</summary>
    public interface Sizable {

        /// <summary>Size of the item</summary>
        public string Size { get; set; }

        /// <summary>Region of the indicated size's measurement</summary>
        public string Region { get; set; }

        //HEYO I had actually made Sizable a generic class and had a size class for letter sizes, male pant sizes, dress shirts, and...
        
        //I have decided that level of complication is not necessary for this project, especially when it wouldn't even be complete because
        //I'm only doing US sizes. So let's leave this flexible for those who use the app.

    }
}
