using Clothespin2.Common.Clothes;
using Clothespin2.Common.Tracking;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Clothespin2.Common {

    /// <summary></summary>
    public class User : Depictable {

        /// <summary>Username of this user</summary>
        [Key]
        public string Username { get; set; } = "";

        /// <summary>Password for this user</summary>
        public string Password { get; set; } = "";
        
        public string ImageURL { get; set; } = "";

        /// <summary>list of wearables this user owns</summary>
        [JsonIgnore]
        public List<Wearable> Wearables = new();

        /// <summary>List of outfits this user owns</summary>
        [JsonIgnore]
        public List<Outfit> Outfits = new();

        /// <summary>List of logs this user has made </summary>
        [JsonIgnore]
        public List<LogItem> LogItems = new();

        /// <summary>Checks a given password for this user</summary>
        /// <param name="Check"></param>
        /// <returns></returns>
        public bool CheckPassword(string Check) => Check == Password;
    }
}
