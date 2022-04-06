using System.ComponentModel.DataAnnotations.Schema;

namespace Clothespin2.Common {

    /// <summary>Abstract class for any ID-able object</summary>
    public abstract class Identifiable {

        //This is an abstract class so I can keep this database generated annotation I'm not entirely sure that it'll work in an interface and I want to save me that import
        //please just let me have this. Everything in these interfaces would be an abstract class but I want to keep things separate just in case

        /// <summary>ID of the given object</summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        /// <summary>Compares if this Identifiable is the same as another identifiable</summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj) => obj is Identifiable I && I.ID.Equals(ID);

        /// <summary>Generates a Hashcode for this Identifiable</summary>
        /// <returns>Delegates to the ID</returns>
        public override int GetHashCode() => ID.GetHashCode();
    }
}
