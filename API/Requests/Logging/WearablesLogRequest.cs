namespace Clothespin2.API.Requests {

    /// <summary>Request to add a log</summary>
    public class WearablesLogRequest : OutfitRequest, LogRequest{

        /// <summary>Date of the log to add</summary>
        public DateTime? Date { get; set; }

        /// <summary>Note attached to the log</summary>
        public string Note { get; set; } = "";

    }
}
