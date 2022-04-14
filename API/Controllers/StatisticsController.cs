using Clothespin2.Common.Clothes;
using Clothespin2.Common.Clothes.Items;
using Clothespin2.Data;
using Igtampe.ChopoSessionManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Clothespin2.API.Controllers {

    /// <summary>Controller for clothing and related operations</summary>
    [Route("API/Statistics")]
    [ApiController]
    public class StatisticsController : ControllerBase {

        #region Constructor and Props

        private readonly ClothespinContext DB;

        /// <summary>Creates a Clothes Controller</summary>
        /// <param name="Context"></param>
        public StatisticsController(ClothespinContext Context) => DB = Context;

        #endregion

        //View list of most used clothing items
        [HttpGet("Wearables")]
        public async Task<IActionResult> WearablesOverall([FromHeader] Guid? SessionID) {

            //Just get a list of all the logging entries
            //Unfortunately a lot of this job will not be able to be done by DB. Do I care however? no.

            return Ok();

        
        }

        /// <summary>Gets a list of most used outfits</summary>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        [HttpGet("Outfits")]
        public async Task<IActionResult> OutfitsOverall([FromHeader] Guid? SessionID) {
            Session? S = await Task.Run(() => SessionManager.Manager.FindSession(SessionID));
            if (S is null) { return BadRequest(ErrorResult.Reusable.InvalidSession); }

            //Get all logging items and group them by the count of the outfits.
            var Data = await DB.LogItem
                .Where(A => A.Owner != null && A.Owner.Username == S.UserID)
                .Where(A => A.Outfit != null && !A.Outfit.Deleted).GroupBy(A => A.Outfit)
                .Select(A => new { A.Key!.ID, A.Key.Name, A.Key.Description, A.Key.ImageURL, Count = A.Count() })
                .OrderByDescending(A => A.Count).ToListAsync();

            //Count the number of deleted outfits
            var DeletedCount = await DB.LogItem.Where(A => A.Outfit != null && A.Outfit.Deleted).CountAsync();
            Data.Add(new { 
                ID = Guid.Empty, Name = "Deleted Outfits", 
                Description = "Outfits that have been deleted", ImageURL = "", 
                Count = DeletedCount });

            var NonOutfits = await DB.LogItem.Where(A => A.Outfit == null).CountAsync();
            Data.Add(new {
                ID = Guid.Empty, Name = "Other",
                Description = "Other outfits that are not tracked. These are from log entries that were created with a list of wearables that " +
                "did not match any existing outfits at the time", ImageURL = "",
                Count = DeletedCount
            });

            return Ok(Data);

        }

        //View list of least used clothing items
        [HttpGet("Unused")]
        public async Task<IActionResult> UnusedWearables() { return Ok(); }

        //View list of unused clothing items

        //The above, by month
        [HttpGet("Wearables/Monthly")]
        public async Task<IActionResult> WearablesByMonth() { return Ok(); }

        /// <summary>Gets a list of most used outfit by month</summary>
        /// <param name="SessionID"></param>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        /// <returns></returns>
        [HttpGet("Outfits/Monthly")]
        public async Task<IActionResult> OutfitsByMonth([FromHeader] Guid? SessionID, [FromQuery] DateTime? Start, [FromQuery] DateTime? End ) {
            Session? S = await Task.Run(() => SessionManager.Manager.FindSession(SessionID));
            if (S is null) { return BadRequest(ErrorResult.Reusable.InvalidSession); }

            //Get all logging items and group them by the count of the outfits.
            var Data = await DB.LogItem
                .Where(A => A.Owner != null && A.Owner.Username == S.UserID)
                .Where(A=>A.Date >= (Start ?? DateTime.UtcNow.AddMonths(-4)) && A.Date <= (End ?? DateTime.UtcNow))
                .Where(A => A.Outfit != null && !A.Outfit.Deleted).GroupBy(A => new { A.Outfit, A.Date!.Value.Year, A.Date.Value.Month })
                .Select(A => new { A.Key.Year, A.Key.Month, A.Key.Outfit!.ID, A.Key.Outfit.Name, A.Key.Outfit.Description, A.Key.Outfit.ImageURL, Count = A.Count() })
                .OrderByDescending(A => A.Count).ToListAsync();

            return Ok(Data);
        }

        [HttpGet("Unused/Monthly")]
        public async Task<IActionResult> UnusedWearablesByMonth() { return Ok(); }

    }
}
