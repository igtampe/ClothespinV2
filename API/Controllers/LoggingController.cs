using Clothespin2.API.Requests;
using Clothespin2.Common.Clothes;
using Clothespin2.Common.Tracking;
using Clothespin2.Data;
using Igtampe.ChopoSessionManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clothespin2.API.Controllers {

    /// <summary>Controller for clothing and related operations</summary>
    [Route("API/Logs")]
    [ApiController]
    public class LoggingController : ControllerBase {

        #region Constructor and Props

        private readonly ClothespinContext DB;

        /// <summary>Creates a Clothes Controller</summary>
        /// <param name="Context"></param>
        public LoggingController(ClothespinContext Context) => DB = Context;

        #endregion

        //View Logs
        /// <summary>Gets a list of all logs from this user</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="Skip">Entries to skip over</param>
        /// <param name="Take">Entries to take</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index([FromHeader] Guid? SessionID, int? Skip, int? Take) {
            Session? S = await Task.Run(() => SessionManager.Manager.FindSession(SessionID));
            if (S is null) { return BadRequest(ErrorResult.Reusable.InvalidSession); }

            var Collection = DB.LogItem
                                .Include(A => A.Outfit).Include(A => A.Shirt)
                                .Include(A => A.Dress).Include(A => A.Pants)
                                .Include(A => A.Shoes).Include(A => A.OuterwearLayers)
                                .Where(A => A.Owner != null && A.Owner.Username == S.UserID)
                                .OrderByDescending(A => A.Date)
                                .Skip(Skip ?? 0).Take(Take ?? 20);

            return Ok(await Collection.ToListAsync());

        }

        //Create Log from Outfit
        /// <summary>Creates a log by outfit ID</summary>
        /// <param name="SessionID"></param>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpPost("ByOutfit")]
        public async Task<IActionResult> CreateOutfitLog([FromHeader] Guid? SessionID, [FromBody] OutfitLogRequest Request) => await CreateLog(SessionID, Request);

        //Create Log from Wearables
        /// <summary>Creates a log by wearables that would go in an outfit. If an outfit with the exact </summary>
        /// <param name="SessionID"></param>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpPost("ByWearables")]
        public async Task<IActionResult> CreateWearableLog([FromHeader] Guid? SessionID, [FromBody] WearablesLogRequest Request) => await CreateLog(SessionID, Request);

        private async Task<IActionResult> CreateLog(Guid? SessionID, LogRequest Request ) {
            Session? S = await Task.Run(() => SessionManager.Manager.FindSession(SessionID));
            if (S is null) { return BadRequest(ErrorResult.Reusable.InvalidSession); }

            LogItem Item = new() {
                Date = Request.Date,
                Note = Request.Note,
            };

            Outfit O;

            if (Request is OutfitLogRequest OLR) {

                try {
                    O = await OutfitsController.InternalGetOutfit(DB, SessionID, OLR.OutfitID ?? Guid.Empty);
                } catch (InvalidOperationException E) {
                    return BadRequest(ErrorResult.BadRequest(E.Message));
                } catch (KeyNotFoundException E) { return NotFound(ErrorResult.BadRequest(E.Message)); }

                Item.Outfit = O;

            } else if (Request is WearablesLogRequest WLR) {

                try {
                    O = await OutfitsController.OutfitRequestToOutfit(DB, SessionID, WLR);
                } catch (InvalidOperationException E) {
                    return BadRequest(ErrorResult.BadRequest(E.Message));
                } catch (KeyNotFoundException E) { return NotFound(ErrorResult.BadRequest(E.Message)); }

                List<Outfit> SimilarOutfits;

                try {
                    SimilarOutfits = await OutfitsController.FindSimilarOutfits(DB, SessionID, WLR, 0, 2);
                } catch (InvalidOperationException E) {
                    return BadRequest(ErrorResult.BadRequest(E.Message));
                } catch (KeyNotFoundException E) { return NotFound(ErrorResult.BadRequest(E.Message)); }

                if (SimilarOutfits.Count == 1) {

                    var P = SimilarOutfits[0];

                    //Verify that the components are equal. This is going to be a massive If
                    //That massive if has been moved to the Outfit class haha

                    if (O.SameWearables(P)) { Item.Outfit = P; }

                }
            } else {
                return BadRequest(ErrorResult.BadRequest("I have no idea what type of request this is please help"));
            }

            Item.Shoes = O.Shoes;
            Item.Pants = O.Pants;
            Item.Dress = O.Dress;
            Item.Shirt = O.Shirt;
            Item.OuterwearLayers = O.OuterwearLayers;

            DB.Add(Item);
            await DB.SaveChangesAsync();

            return Ok(Item);

        }
    }
}
