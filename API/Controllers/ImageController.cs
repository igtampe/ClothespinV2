using Microsoft.AspNetCore.Mvc;
using Clothespin2.Common;
using Clothespin2.Data;
using Igtampe.ChopoSessionManager;

namespace Clothespin2.API.Controllers {
    
    /// <summary>Controller that handles User operations</summary>
    [Route("API/Images")]
    [ApiController]
    public class ImageController : ControllerBase {

        private readonly ClothespinContext DB;

        /// <summary>Creates a User Controller</summary>
        /// <param name="Context"></param>
        public ImageController(ClothespinContext Context) => DB = Context;

        /// <summary>Gets an image from the DB</summary>
        /// <param name="ID">ID of the image to retrieve</param>
        /// <returns></returns>
        [HttpGet("{ID}")]
        public async Task<IActionResult> GetImage(Guid ID) {

            Image? I = await DB.Image.FindAsync(ID);
            return I is null || I.Data is null || I.Type is null ? NotFound("Image was not found") : File(I.Data, I.Type);
        }

        /// <summary>Uploads an Image to the DB.</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <returns></returns>
        // POST api/Images
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromHeader] Guid SessionID) {

            //Check the session:
            Session? S = await Task.Run(() => SessionManager.Manager.FindSession(SessionID));
            if (S is null) { return BadRequest(ErrorResult.Reusable.InvalidSession); }

            //That is all we will check the session for. In some future other project we may have an `Image Uploader` role to verify but for now this is fine
            string? ContentType = Request.ContentType;

            int MaxSize = 1024*1024*1;

            if (ContentType != "image/png" && ContentType != "image/jpeg" && ContentType != "image/gif") { return BadRequest("File must be PNG, JPG, or GIF"); }
            if (Request.ContentLength > MaxSize) { return BadRequest(ErrorResult.BadRequest("Image must be less than 1MB")); } //Add an extra 1 byte just in case

            Image I = new() { Type = ContentType };

            // Get the temporary folder, and combine a random file name with it
            using (var memoryStream = new MemoryStream()) {
                await Request.Body.CopyToAsync(memoryStream);
                I.Data = memoryStream.ToArray();
                if (I.Data.Length > MaxSize) { return BadRequest(ErrorResult.BadRequest("Image must be less than 1MB")); }
            }

            DB.Image.Add(I);
            await DB.SaveChangesAsync();

            return Ok(I);

        }
    }
}
