namespace Clothespin2.Common.Clothes {

    //Hehe this holds all the types. Maybe it should've been a class, but putting this in a namespace makes it easier to import.
    //This might break some convention but oops.

    /// <summary>Static class that holds Clothespin's recognized wearabe subtypes. This is really only here for reference as the 
    /// frontend should have this locally. Remove this when its done</summary>
    public static class Types {

        public static readonly string[] Shirts = { "Other", "Tank Top", "T-Shirt", "Polo", "Shirt", "Dress Shirt", "Sweatshirt" };

        public static readonly string[] OuterwearLayers = { "Other", "Jacket", "Coat", "Sweater", "Suit", "Vest", "Blazer" };

        //Emma pls u know more about dresses than I do pls verify that this is even remotely ok.
        //Wanna know a fun fact? Dress was included in the original clothespin. As a subtype for shirt :dance:
        public static readonly string[] Dresses = { "Other", "Shift", "A-Line", "Bodycon", "Tent", "1 Shoulder", "Apron", "Jumper", "Sun Dress", "Wrap", "Slip", "Shirt Dress", "Maxi", "Ball Gown" };

        public static readonly string[] Pants = { "Other", "Shorts", "Skirt", "Dress Skirt", "Jeans", "Khakis", "Dress Pants", "Sweatpants", "Sport Shorts" };

        public static readonly string[] Shoes = { "Other", "Sandals", "Loafers", "Slippers", "Formal", "Work", "Boots", "Heels", "Sneakers", "Tennis" };

    }
}
