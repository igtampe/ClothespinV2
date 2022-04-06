namespace Clothespin2.Common {

    /// <summary>Image class that holds an image in the database</summary>
    public class Image : Identifiable {

        /// <summary>Data of the image</summary>
        public byte[]? Data { get; set; }

        /// <summary>MIME Type of this image (image/png, image/jpeg, or image/gif)</summary>
        public string? Type { get; set; }
    }
}
