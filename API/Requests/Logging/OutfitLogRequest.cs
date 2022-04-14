namespace Clothespin2.API.Requests {

    /// <summary>Request to add a log</summary>
    public class OutfitLogRequest : LogRequest {

        /// <summary>ID of the outfit to log</summary>
        public Guid? OutfitID { get; set; }

        /// <summary>Date of the log to add</summary>
        public DateTime? Date { get; set; }

        /// <summary>Note attached to the log</summary>
        public string Note { get; set; } = "";

    }
}
