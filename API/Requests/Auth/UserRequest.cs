namespace Clothespin2.API.Requests {

    /// <summary>Request with a username and a password (either to check or register)</summary>
    public class UserRequest {

        /// <summary>Username of this request</summary>
        public string? Username { get; set; }

        /// <summary>Password of this request</summary>
        public string? Password { get; set; }
    }
}
