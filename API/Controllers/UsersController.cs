using Microsoft.AspNetCore.Mvc;
using Clothespin2.Common;
using Clothespin2.Data;
using Igtampe.ChopoSessionManager;
using Clothespin2.API.Requests;
using Microsoft.EntityFrameworkCore;

namespace Clothespin2.API.Controllers {
    
    /// <summary>Controller that handles User operations</summary>
    [Route("API/Users")]
    [ApiController]
    public class UsersController : ControllerBase {

        private readonly ClothespinContext DB;

        /// <summary>Creates a User Controller</summary>
        /// <param name="Context"></param>
        public UsersController(ClothespinContext Context) => DB = Context;

        // POST api/Users
        /// <summary>Handles logging in to Clothespin</summary>
        /// <param name="Request">Request with a User and Password attempt to log in</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> LogIn(UserRequest Request) {
            if (Request.Username is null || Request.Password is null) { return BadRequest("Username or Password is null"); }

            //Check the user on the DB instead of the user de-esta cosa
            bool Login = await DB.User.AnyAsync(U => U.Username == Request.Username && U.Password == Request.Password);
            if (!Login) { return BadRequest("Username or Password is wrong"); }

            //Generate a session
            return Ok(SessionManager.Manager.LogIn(Request.Username));
           
        }

        /// <summary>Handles user registration</summary>
        /// <param name="Request">User and password combination to create</param>
        /// <returns></returns>
        // POST api/Users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRequest Request) {
            if (Request.Username is null || Request.Password is null) { return BadRequest("Username or Password is null"); }

            bool UserExists = await DB.User.AnyAsync(u => u.Username == Request.Username);
            if (UserExists) { return BadRequest("User already exists!"); }

            DB.User.Add(new() {
                Username = Request.Username,
                Password = Request.Password,
            });

            await DB.SaveChangesAsync();
            return Ok();

        }

        /// <summary>Gets the data of the currently logged in user</summary>
        /// <param name="SessionID">ID of the session</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Me([FromHeader] Guid? SessionID) {
            Session? S = await Task.Run(() => SessionManager.Manager.FindSession(SessionID ?? Guid.Empty));
            return S is null
                ? BadRequest(ErrorResult.Reusable.InvalidSession)
                : Ok(await DB.User.FirstOrDefaultAsync(A => A.Username == S.UserID));
        }

        /// <summary>Handles user password changes</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="Request">Request with a logged in user's session, and their current and new passwords</param>
        /// <returns></returns>
        // PUT api/Users
        [HttpPut]
        public async Task<IActionResult> Update([FromHeader] Guid? SessionID, ChangePasswordRequest Request) {

            //Ensure nothing is null
            if (Request.New is null) { return BadRequest("Cannot have empty password"); }

            //Check the session:
            Session? S = await Task.Run(()=>SessionManager.Manager.FindSession(SessionID));
            if (S is null) { return BadRequest("Invalid session"); }

            //Check the password
            User? U = await DB.User.FirstOrDefaultAsync(U => U.Username == S.UserID && U.Password == Request.Current);
            if (U is null) { return BadRequest("Incorrect current password"); }

            U.Password = Request.New;
            DB.Update(U);
            await DB.SaveChangesAsync();

            return Ok();

        }

        /// <summary>Handles user password changes</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="Request">Request with a logged in user's session, and their current and new passwords</param>
        /// <returns></returns>
        // PUT api/Users
        [HttpPut("/image")]
        public async Task<IActionResult> UpdateImage([FromHeader] Guid? SessionID, UserImageRequest Request) {

            //Check the session:
            Session? S = await Task.Run(() => SessionManager.Manager.FindSession(SessionID));
            if (S is null) { return BadRequest("Invalid session"); }

            //Check the password
            User? U = await DB.User.FirstOrDefaultAsync(U => U.Username == S.UserID);
            if (U is null) { return BadRequest("Invalid Session"); }

            U.ImageURL = Request.ImageURL;
            DB.Update(U);
            await DB.SaveChangesAsync();

            return Ok();

        }

        /// <summary>Handles user logout</summary>
        /// <param name="SessionID">Session to log out of</param>
        /// <returns></returns>
        // POST api/Users/out
        [HttpPost("out")]
        public async Task<IActionResult> LogOut([FromHeader] Guid SessionID) => Ok(await Task.Run(() => SessionManager.Manager.LogOut(SessionID)));

        /// <summary>Handles user logout of *all* sessions</summary>
        /// <param name="SessionID">Session that wants to log out of all tied sessions</param>
        /// <returns></returns>
        // POST api/Users/outall
        [HttpPost("outall")]
        public async Task<IActionResult> LogOutAll([FromBody] Guid SessionID) {
            //Check the session:
            Session? S = await Task.Run(() => SessionManager.Manager.FindSession(SessionID));
            return S is null ? BadRequest("Invalid session") : Ok(await Task.Run(() => SessionManager.Manager.LogOutAll(S.UserID)));
        }
    }
}
