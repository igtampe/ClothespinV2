using Clothespin2.API.Requests;
using Clothespin2.Common;
using Clothespin2.Common.Clothes;
using Clothespin2.Common.Clothes.Items;
using Clothespin2.Data;
using Igtampe.ChopoSessionManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Clothespin2.API.Controllers {

    /// <summary>Controller for clothing and related operations</summary>
    [Route("API/Outfits")]
    [ApiController]
    public class OutfitsController : ControllerBase {

        #region Constructor and Props

        private readonly ClothespinContext DB;

        /// <summary>Creates a Clothes Controller</summary>
        /// <param name="Context"></param>
        public OutfitsController(ClothespinContext Context) => DB = Context;

        #endregion

        #region View Outfit

        //View Outfit
        /// <summary>Gets an individual outfit</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="ID">ID of the outfit</param>
        /// <returns></returns>
        [HttpGet("{ID}")]
        public async Task<IActionResult> GetOutfit([FromHeader] Guid? SessionID, [FromRoute] Guid ID) {
            try { return Ok(await InternalGetOutfit(DB, SessionID, ID));
            } catch (InvalidOperationException E) { return BadRequest(ErrorResult.BadRequest(E.Message));
            } catch (KeyNotFoundException E) { return NotFound(ErrorResult.BadRequest(E.Message)); }
        }

        /// <summary>The superior internal get</summary>
        /// <returns></returns>
        public static async Task<Outfit> InternalGetOutfit(ClothespinContext DB, Guid? SessionID, Guid ID) {
            Session? S = await Task.Run(() => SessionManager.Manager.FindSession(SessionID));
            if (S is null) { throw new InvalidOperationException("Session not valid"); }

            var O = await DB.Outfit
                .Include(A => A.Shirt).Include(A => A.Dress).Include(A => A.Pants)
                .Include(A => A.Shoes).Include(A => A.OuterwearLayers)
                .FirstOrDefaultAsync(A => A.ID == ID && A.Owner != null && A.Owner.Username == S.UserID && !A.Deleted);

            return O is null
                ? throw new KeyNotFoundException("Outfit was not found")
                : O;
        }

        #endregion

        #region Search Outfits

        //Search Outfit

        /// <summary>Gets a list of a user's outfits (Searching if necessary)</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="Query">String to search for in Name and Description of this outfit</param>
        /// <param name="Skip">Entries to skip over (IE How many entries to skip over in the list of all outfits)</param>
        /// <param name="Take">Entries to take (IE How many entries to take from the list and actually return)</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SearchOutfit([FromHeader] Guid? SessionID, [FromQuery] string? Query, [FromQuery] int? Skip, [FromQuery] int? Take) {

            Session? S = await Task.Run(() => SessionManager.Manager.FindSession(SessionID));
            if (S is null) { return BadRequest(ErrorResult.Reusable.InvalidSession); }

            var Collection = DB.Outfit
                .Include(A => A.Shirt).Include(A => A.Dress).Include(A => A.Pants)
                .Include(A => A.Shoes).Include(A => A.OuterwearLayers)
                .Where(A => A.Owner != null && A.Owner.Username == S.UserID && !A.Deleted);
            
            if (Query is not null) {
                Query = Query.ToLower();
                Collection = Collection.Where(A => A.Name.ToLower().Contains(Query) || A.Description.ToLower().Contains(Query));
            }
            
            Collection = Collection.OrderBy(A=>A.Name).Skip(Skip ?? 0).Take(Take ?? 20);
            return Ok(await Collection.ToListAsync());
        }

        /// <summary>Gets a list of user's outfits containing wearables in given request</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="Request">Request containing IDs of wearables that must be in outfits in the list this method returns</param>
        /// <param name="Skip">Items to skip over</param>
        /// <param name="Take">Items to take</param>
        /// <returns></returns>
        [HttpPost("Search")]
        public async Task<IActionResult> SearchOutfitByWearables([FromHeader] Guid? SessionID, [FromBody] OutfitRequest Request, [FromQuery] int? Skip, [FromQuery] int? Take) {
            try { return Ok(await FindSimilarOutfits(DB,SessionID,Request,Skip,Take));
            } catch (InvalidOperationException E) { return BadRequest(ErrorResult.BadRequest(E.Message));
            } catch (KeyNotFoundException E) { return NotFound(ErrorResult.BadRequest(E.Message)); }
        }

        #endregion

        #region Create Outfits

        //Create outfit
        /// <summary>Creates an outfit</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="Request">Request containing details of the outfit to create, and wearables contained in this outfit</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateOutfit([FromHeader] Guid? SessionID, [FromBody] OutfitCreateRequest Request) {
            Outfit O;

            try { O = await OutfitRequestToOutfit(DB, SessionID, Request);
            } catch (InvalidOperationException E) {return BadRequest(ErrorResult.BadRequest(E.Message));
            } catch (KeyNotFoundException E) { return NotFound(ErrorResult.BadRequest(E.Message)); }

            DB.Outfit.Add(O);
            await DB.SaveChangesAsync();
            return Ok(O);

        }

        #endregion

        #region Update Outfits

        //Update Outfit
        /// <summary>Updates an outfit</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="ID">ID of the outfit to update</param>
        /// <param name="Request">Request containing all new details for the outfit</param>
        /// <returns></returns>
        [HttpPut("{ID}")]
        public async Task<IActionResult> UpdateOutfit([FromHeader] Guid? SessionID, [FromRoute] Guid ID, [FromBody] OutfitCreateRequest Request) {

            //Get the current outfit
            
            Outfit Existing;

            try { Existing = await InternalGetOutfit(DB, SessionID, ID);
            } catch (InvalidOperationException E) { return BadRequest(ErrorResult.BadRequest(E.Message));
            } catch (KeyNotFoundException E) { return NotFound(ErrorResult.BadRequest(E.Message)); }

            //Get the updated outfit

            Outfit Updated;

            try { Updated = await OutfitRequestToOutfit(DB, SessionID, Request);
            } catch (InvalidOperationException E) { return BadRequest(ErrorResult.BadRequest(E.Message));
            } catch (KeyNotFoundException E) { return NotFound(ErrorResult.BadRequest(E.Message)); }

            //Update all props
            foreach (PropertyInfo Prop in typeof(Outfit).GetProperties()) {

                //Check for the properties we need to skip
                switch (Prop.Name.ToUpper()) {
                    case "ID":
                    case "OWNER":
                        continue;
                    default:
                        break;
                }

                //Get the updated value
                object? O = Prop.GetValue(Updated);

                //Update the value as long as its not null or whitespace
                if (O is not null) { Prop.SetValue(Existing, O); }
            }

            DB.Outfit.Update(Existing);
            await DB.SaveChangesAsync();
            return Ok(Existing);

        }

        #endregion

        #region Delete Outfits

        //Delete Outfit
        /// <summary>Deletes an outfit</summary>
        /// <param name="SessionID">ID of the session</param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete("{ID}")]
        public async Task<IActionResult> Delete([FromHeader] Guid? SessionID, [FromRoute] Guid ID) {

            Outfit O;

            try { O = await InternalGetOutfit(DB, SessionID, ID);
            } catch (InvalidOperationException E) { return BadRequest(ErrorResult.BadRequest(E.Message));
            } catch (KeyNotFoundException E) { return NotFound(ErrorResult.BadRequest(E.Message)); }

            DB.Remove(O);
            await DB.SaveChangesAsync();
            return Ok(O);

        }

        #endregion

        #region Static Helper Methods

        /// <summary>Gets a list of outfits containing all of the wearables listed on the request</summary>
        /// <param name="DB"></param>
        /// <param name="SessionID"></param>
        /// <param name="Request"></param>
        /// <param name="Skip"></param>
        /// <param name="Take"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async static Task<List<Outfit>> FindSimilarOutfits(ClothespinContext DB, Guid? SessionID, OutfitRequest Request, int? Skip, int? Take) {

            Session? S = await Task.Run(() => SessionManager.Manager.FindSession(SessionID));
            if (S is null) { throw new InvalidOperationException("Session not valid"); }

            var Collection = DB.Outfit
                .Include(A => A.Shirt).Include(A => A.Dress).Include(A => A.Pants)
                .Include(A => A.Shoes).Include(A => A.OuterwearLayers)
                .Where(A => A.Owner != null && A.Owner.Username == S.UserID && !A.Deleted);

            if (Request.ShirtID is not null) { Collection = Collection.Where(A => A.Shirt != null && A.Shirt.ID == Request.ShirtID); }
            if (Request.DressID is not null) { Collection = Collection.Where(A => A.Dress != null && A.Dress.ID == Request.DressID); }
            if (Request.PantsID is not null) { Collection = Collection.Where(A => A.Pants != null && A.Pants.ID == Request.PantsID); }
            if (Request.ShoesID is not null) { Collection = Collection.Where(A => A.Shoes != null && A.Shoes.ID == Request.ShoesID); }

            if (Request.OuterwearIDs is not null && Request.OuterwearIDs.Length > 0) {
                Request.OuterwearIDs.ToList().ForEach(ID => Collection = Collection.Where(O => O.OuterwearLayers.Any(OS => OS.ID == ID)));
            }

            Collection = Collection.OrderBy(A => A.Name).Skip(Skip ?? 0).Take(Take ?? 20);
            return await Collection.ToListAsync();
            
        }

        //Outfit Request to Outfit
        /// <summary></summary>
        /// <param name="DB"></param>
        /// <param name="SessionID"></param>
        /// <param name="Request"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If the session is not valid</exception>
        /// <exception cref="KeyNotFoundException">If a key for a wearable was not found</exception>
        public async static Task<Outfit> OutfitRequestToOutfit(ClothespinContext DB, Guid? SessionID, OutfitRequest Request) {

            Session? S = await Task.Run(() => SessionManager.Manager.FindSession(SessionID));
            if (S is null) { throw new InvalidOperationException("Session not valid"); }

            User? U = await DB.User.FirstOrDefaultAsync(A => A.Username == S.UserID);
            var O = new Outfit { Owner = U };

            if (Request is OutfitCreateRequest OCR) { 
                O.Name= OCR.Name;
                O.Description= OCR.Description;
                O.ImageURL= OCR.ImageURL;
            };

            //We must find each individual de-esta cosa
            if (Request.ShirtID is not null) { O.Shirt = await GetWearableOfType<Shirt>()(DB, S, Request.ShirtID); } //What monstrosity have I created
            if (Request.DressID is not null) { O.Dress = await GetWearableOfType<Dress>()(DB, S, Request.DressID); } //I hope this works
            if (Request.PantsID is not null) { O.Pants = await GetWearableOfType<Pants>()(DB, S, Request.PantsID); }
            if (Request.ShoesID is not null) { O.Shoes = await GetWearableOfType<Shoes>()(DB, S, Request.ShoesID); }

            if (Request.OuterwearIDs is not null && Request.OuterwearIDs.Length > 0) {
                foreach (Guid LayerID in Request.OuterwearIDs) {
                    O.OuterwearLayers.Add(await GetWearableOfType<Outerwear>()(DB, S, LayerID));                    
                }
            }

            return O;
        }

        /// <summary>Maybe the other functoin didn't need to exist but this function makes me feel super smart</summary>
        /// <typeparam name="E"></typeparam>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        private static Func<ClothespinContext, Session, Guid?, Task<E>> GetWearableOfType<E>() where E : Wearable {
            return (async (DB,S,ItemID) => {
                Wearable?  W = await InternalGetWearable(DB, S, ItemID);
                
                return W is null
                    ? throw new KeyNotFoundException($"{nameof(E)} was not found")
                    : W is not E Item 
                        ? throw new InvalidCastException($"{nameof(E)} ID did not point to a {nameof(E)}") 
                        : Item;
            });
        }

        /// <summary>Hhahahahaahahahah this shouldn't be necessary pero pues oops. This method should only be called by <see cref="OutfitRequestToOutfit(ClothespinContext, Guid?, OutfitRequest)"/> because it assumes that the session has already been verified</summary>
        /// <param name="DB"></param>
        /// <param name="S"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        private static async Task<Wearable?> InternalGetWearable(ClothespinContext DB, Session S, Guid? ID) {
            Wearable? B = await DB.Wearable.FirstOrDefaultAsync(A => A.ID == ID && A.Owner != null && A.Owner.Username == S.UserID && !A.Deleted);
            return null;
        }

        #endregion 

    }
}
