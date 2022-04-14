using Clothespin2.Common.Clothes;
using Clothespin2.Common.Clothes.Items;
using Clothespin2.Data;
using Igtampe.ChopoSessionManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Clothespin2.API.Controllers {

    /// <summary>Sort orders for Wearables</summary>
    public enum WearableSortType {

        /// <summary>Sort by Name ascending</summary>
        BY_NAME = 0,

        /// <summary>Sort by name descending</summary>
        BY_NAME_DESC = 1,

        /// <summary>Sort by Type ascending</summary>
        BY_TYPE = 2,

        /// <summary>Sort by type descending</summary>
        BY_TYPE_DESC = 3,
    }

    /// <summary>Controller for clothing and related operations</summary>
    [Route("API/Clothes")]
    [ApiController]
    public class ClothesController : ControllerBase {

        #region Constructor and Props

        private readonly ClothespinContext DB;

        /// <summary>Creates a Clothes Controller</summary>
        /// <param name="Context"></param>
        public ClothesController(ClothespinContext Context) => DB = Context;

        #endregion

        #region View Wearables

        //View Shirt
        //View Outerwear
        //View Dress
        //View Pants
        //View Shoes
        /// <summary>Gets any type of wearable and returns it</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="ID">ID of the wearable</param>
        /// <returns></returns>
        [HttpGet("{ID}")]
        public async Task<IActionResult> GetWearable([FromHeader] Guid? SessionID, [FromRoute] Guid ID) {
            var R = await InternalGetWearable(SessionID, ID);
            return R.Item2 is not null ? R.Item2 : Ok(R.Item1 as object);
        }
        private async Task<(Wearable?, IActionResult?)> InternalGetWearable(Guid? SessionID, Guid ID) {
            Session? S = await Task.Run(() => SessionManager.Manager.FindSession(SessionID));
            if (S is null) { return new(null, BadRequest(ErrorResult.Reusable.InvalidSession)); }

            Wearable? B = await DB.Wearable.FirstOrDefaultAsync(A => A.ID == ID && A.Owner != null && A.Owner.Username == S.UserID && !A.Deleted);
            return new(B, B is null ? NotFound(ErrorResult.NotFound("Item was not found")) : null);
        }

        #endregion

        #region Search Wearables
        //Search Shirt
        /// <summary>Gets a list of shirts</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="State">WashState to show</param>
        /// <param name="SortType">Sort order of the list</param>
        /// <param name="Query">String query to find in name or description</param>
        /// <param name="Skip">Items to skip</param>
        /// <param name="Take">Items to take</param>
        /// <returns></returns>
        [HttpGet("Shirts")]
        public async Task<IActionResult> GetShirts([FromHeader] Guid? SessionID, [FromQuery] WashState? State, [FromQuery] WearableSortType? SortType,
            [FromQuery] string? Query, [FromQuery] int? Skip, [FromQuery] int? Take)
            => await GetWashableList(SessionID, State, DB.Shirt, SortType, Query, Skip, Take);

        //Search Otuerwear
        /// <summary>Gets a list of overshirts</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="State">WashState to show</param>
        /// <param name="SortType">Sort order of the list</param>
        /// <param name="Query">String query to find in name or description</param>
        /// <param name="Skip">Items to skip</param>
        /// <param name="Take">Items to take</param>
        /// <returns></returns>
        [HttpGet("Outerwears")]
        public async Task<IActionResult> GetOuterwear([FromHeader] Guid? SessionID, [FromQuery] WashState? State, [FromQuery] WearableSortType? SortType,
            [FromQuery] string? Query, [FromQuery] int? Skip, [FromQuery] int? Take)
            => await GetWashableList(SessionID, State, DB.Outerwear, SortType, Query, Skip, Take);

        //Search Dress
        /// <summary>Gets a list of Dresses</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="State">WashState to show</param>
        /// <param name="SortType">Sort order of the list</param>
        /// <param name="Query">String query to find in name or description</param>
        /// <param name="Skip">Items to skip</param>
        /// <param name="Take">Items to take</param>
        /// <returns></returns>
        [HttpGet("Dresses")]
        public async Task<IActionResult> GetDress([FromHeader] Guid? SessionID, [FromQuery] WashState? State, [FromQuery] WearableSortType? SortType,
            [FromQuery] string? Query, [FromQuery] int? Skip, [FromQuery] int? Take)
            => await GetWashableList(SessionID, State, DB.Dress, SortType, Query, Skip, Take);

        //Search Pants
        /// <summary>Gets a list of pants</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="State">WashState to show</param>
        /// <param name="SortType">Sort order of the list</param>
        /// <param name="Query">String query to find in name or description</param>
        /// <param name="Skip">Items to skip</param>
        /// <param name="Take">Items to take</param>
        /// <returns></returns>
        [HttpGet("Pants")]
        public async Task<IActionResult> GetPants([FromHeader] Guid? SessionID, [FromQuery] WashState? State, [FromQuery] WearableSortType? SortType,
            [FromQuery] string? Query, [FromQuery] int? Skip, [FromQuery] int? Take)
            => await GetWashableList(SessionID, State, DB.Pants, SortType, Query, Skip, Take);

        //Search Shoes
        /// <summary>Gets a list of shoes</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="SortType">Sort order of the list</param>
        /// <param name="Query">String query to find in name or description</param>
        /// <param name="Skip">Items to skip</param>
        /// <param name="Take">Items to take</param>
        /// <returns></returns>
        [HttpGet("Shoes")]
        public async Task<IActionResult> GetShoes([FromHeader] Guid? SessionID, [FromQuery] WearableSortType? SortType,
            [FromQuery] string? Query, [FromQuery] int? Skip, [FromQuery] int? Take)
            => await GetWearableList(SessionID, DB.Shoes, SortType, Query, Skip, Take);

        private async Task<IActionResult> GetWearableList<E>(Guid? SessionID, IQueryable<E> Collection, WearableSortType? SortType, string? Query, int? Skip, int? Take) where E : Wearable {
            Session? S = await Task.Run(() => SessionManager.Manager.FindSession(SessionID));
            if (S is null) { return BadRequest(ErrorResult.Reusable.InvalidSession); }

            Collection = Collection.Where(A => A.Owner != null && A.Owner.Username == S.UserID && !A.Deleted);
            if (Query is not null) {
                Query = Query.ToLower();
                Collection = Collection.Where(A => A.Name.ToLower().Contains(Query) || A.Description.ToLower().Contains(Query));
            }

            if (SortType is not null) {
                Collection = SortType switch {
                    WearableSortType.BY_NAME => Collection.OrderBy(A => A.Name),
                    WearableSortType.BY_NAME_DESC => Collection.OrderByDescending(A => A.Name),
                    WearableSortType.BY_TYPE => Collection.OrderBy(A => A.Subtype).ThenBy(A => A.Name),
                    WearableSortType.BY_TYPE_DESC => Collection.OrderByDescending(A => A.Subtype).ThenBy(A => A.Name),
                    _ => throw new InvalidOperationException("Uh.... This should be impossible"),
                };
            }

            Collection = Collection.Skip(Skip ?? 0).Take(Take ?? 20);
            return Ok(await Collection.ToListAsync());

        }

        /// <summary>Gets a list of washables</summary>
        /// <typeparam name="E">Washable type</typeparam>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="State">WashState to show</param>
        /// <param name="Collection">Collection of wearable type</param>
        /// <param name="SortType">Sort order of the list</param>
        /// <param name="Query">String query to find in name or description</param>
        /// <param name="Skip">Items to skip</param>
        /// <param name="Take">Items to take</param>
        /// <returns></returns>
        private async Task<IActionResult> GetWashableList<E>(Guid? SessionID, WashState? State, IQueryable<E> Collection, WearableSortType? SortType, string? Query, int? Skip, int? Take) where E : Washable {
            if (State is not null) { Collection = Collection.Where(A => A.State == State); }
            return await GetWearableList<E>(SessionID, Collection, SortType, Query, Skip, Take);
        }

        #endregion

        #region Create Wearables
        //Create Shirt
        /// <summary>Creates a shirt</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="Item">Item to create</param>
        /// <returns></returns>
        [HttpPost("Shirts")]
        public async Task<IActionResult> CreateShirt(Guid? SessionID, Shirt Item) => await CreateWearable(SessionID, Item);

        //Create Outerwear
        /// <summary>Creates an Outerwear layer</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="Item">Item to create</param>
        /// <returns></returns>
        [HttpPost("Outerwears")]
        public async Task<IActionResult> CreateOuterwear(Guid? SessionID, Outerwear Item) => await CreateWearable(SessionID, Item);

        //Create Dress
        /// <summary>Creates a Dress</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="Item">Item to create</param>
        /// <returns></returns>
        [HttpPost("Dresses")]
        public async Task<IActionResult> CreateDress(Guid? SessionID, Dress Item) => await CreateWearable(SessionID, Item);

        //Create Pants
        /// <summary>Creates a pair of paints</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="Item">Item to create</param>
        /// <returns></returns>
        [HttpPost("Pants")]
        public async Task<IActionResult> CreatePants(Guid? SessionID, Pants Item) => await CreateWearable(SessionID, Item);

        //Create Shoes
        /// <summary>Creates a pair of shoes</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="Item">Item to create</param>
        /// <returns></returns>
        [HttpPost("Shoes")]
        public async Task<IActionResult> CreateShoes(Guid? SessionID, Shoes Item) => await CreateWearable(SessionID, Item);

        /// <summary>Creates a Wearable</summary>
        /// <typeparam name="E">Type of item to create</typeparam>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="Item">Item to create</param>
        /// <returns></returns>
        private async Task<IActionResult> CreateWearable<E>(Guid? SessionID, E Item) where E : Wearable {
            Session? S = await Task.Run(() => SessionManager.Manager.FindSession(SessionID));
            if (S is null) { return BadRequest(ErrorResult.Reusable.InvalidSession); }

            Item.Owner = await DB.User.FirstOrDefaultAsync(A => A.Username == S.UserID);
            Item.ID = Guid.Empty; //Make sure this is done

            DB.Add(Item);
            await DB.SaveChangesAsync();
            
            return Ok(Item);
        }

        #endregion

        #region Update Wearables
        //Update Shirt
        /// <summary>Creates a shirt</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="Item">Item to create</param>
        /// <returns></returns>
        [HttpPut("Shirts")]
        public async Task<IActionResult> UpdateShirt(Guid? SessionID, Shirt Item) => await UpdateWearable(SessionID, Item);

        //Create Outerwear
        /// <summary>Creates an Outerwear layer</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="Item">Item to create</param>
        /// <returns></returns>
        [HttpPut("Outerwears")]
        public async Task<IActionResult> UpdateOuterwear(Guid? SessionID, Outerwear Item) => await UpdateWearable(SessionID, Item);

        //Create Dress
        /// <summary>Creates a Dress</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="Item">Item to create</param>
        /// <returns></returns>
        [HttpPut("Dresses")]
        public async Task<IActionResult> UpdateDress(Guid? SessionID, Dress Item) => await UpdateWearable(SessionID, Item);

        //Create Pants
        /// <summary>Creates a pair of paints</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="Item">Item to create</param>
        /// <returns></returns>
        [HttpPut("Pants")]
        public async Task<IActionResult> UpdatePants(Guid? SessionID, Pants Item) => await UpdateWearable(SessionID, Item);

        //Create Shoes
        /// <summary>Creates a pair of shoes</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="Item">Item to create</param>
        /// <returns></returns>
        [HttpPut("Shoes")]
        public async Task<IActionResult> UpdateShoes(Guid? SessionID, Shoes Item) => await UpdateWearable(SessionID, Item);

        /// <summary>Creates a Wearable</summary>
        /// <typeparam name="E">Type of item to create</typeparam>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="Item">Item to create</param>
        /// <returns></returns>
        private async Task<IActionResult> UpdateWearable<E>(Guid? SessionID, E Item) where E : Wearable {
            var R = await InternalGetWearable(SessionID, Item.ID);
            if (R.Item1 is null) return R.Item2 ?? throw new InvalidOperationException("This shouldn't happen");

            if (R.Item1.Type != Item.Type) { return BadRequest(ErrorResult.BadRequest("Item type mismatch")); }

            //Update all props:
            foreach (PropertyInfo Prop in typeof(E).GetProperties()) {

                //Check for the properties we need to skip
                switch (Prop.Name.ToUpper()) {
                    case "ID":
                    case "OUTFITS":
                    case "OWNER":
                        continue;
                    default:
                        break;
                }

                //Get the updated value
                object? O = Prop.GetValue(Item);

                //Update the value as long as its not null or whitespace
                if (O is not null) { Prop.SetValue(R.Item1, O); }

                //This alone should be able to handle any type of wearable, washable, and even sizeable creo.
                //Which good god, has made this one of the most *powerful* methods I have written :flushed:
            }

            DB.Update(R.Item1);
            await DB.SaveChangesAsync();

            return Ok(Item);
        }
        #endregion

        #region Delete Wearables
        //Delete Shirt
        //Delete Outerwear
        //Delete Dress
        //Delete Pants
        //Delete Shoes

        /// <summary>Deletes given wearable</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="ID">ID of the wearable</param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteWearable([FromHeader] Guid? SessionID, [FromRoute] Guid ID) {

            var R = await InternalGetWearable(SessionID, ID);
            if(R.Item1 is null) return R.Item2 ?? throw new InvalidOperationException("This shouldn't happen");

            R.Item1.Deleted = true;
            DB.Update(R.Item1);
            await DB.SaveChangesAsync();

            return Ok(R.Item1 as object);

        }

        #endregion

        #region Outfits Associated
        //View Outfits Associated WIth Shirt
        //View Outfits Associated WIth Outerwear
        //View Outfits Associated WIth Dress
        //View Outfits Associated WIth Pants
        //View Outfits Associated WIth Shoes

        /// <summary>Gets a list of outfits which contain this wearable</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="ID">ID of the wearable</param>
        /// <returns></returns>
        [HttpGet("{ID}/Outfits")]
        public async Task<IActionResult> GetOutfitsRelatedToWearable([FromHeader] Guid? SessionID, [FromRoute] Guid ID) {
            Session? S = await Task.Run(() => SessionManager.Manager.FindSession(SessionID));
            if (S is null) { return BadRequest(ErrorResult.Reusable.InvalidSession); }

            Wearable? B = await DB.Wearable
                .Include(A=>A.Outfits)
                .FirstOrDefaultAsync(A => A.ID == ID && A.Owner != null && A.Owner.Username == S.UserID && !A.Deleted);
            
            return B is null 
                ? NotFound(ErrorResult.NotFound("Item was not found")) 
                : Ok(B.Outfits);
        }

        #endregion

    }
}
