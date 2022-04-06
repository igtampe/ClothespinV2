using Clothespin2.Common;
using Clothespin2.Common.Clothes;
using Clothespin2.Common.Clothes.Items;
using Clothespin2.Common.Tracking;
using Microsoft.EntityFrameworkCore;

namespace Clothespin2.Data {
    public class ClothespinContext : PostgresContext {

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DbSet<User> User { get; set; }

        public DbSet<Image> Image { get; set; }

        public DbSet<LogItem> LogItem { get; set; }

        public DbSet<Outfit> Outfit { get; set; }

        public DbSet<Wearable> Wearable { get; set; }

        public DbSet<Shirt> Shirt { get; set; }

        public DbSet<Dress> Dress { get; set; }

        public DbSet<Outerwear> Outerwear { get; set; }

        public DbSet<Pants> Pants { get; set; }

        public DbSet<Shoes> Shoes { get; set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    }
}
