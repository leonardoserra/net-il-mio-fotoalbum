using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Areas.Identity.Data;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers.API
{
    [Route("api/[controller]/[action]")]

    [ApiController]
    public class MessageAPIController : ControllerBase
    {
        private PhotoAlbumsContext _db = new PhotoAlbumsContext();
        //private PhotoAlbumsContext _UserDb = new PhotoAlbumsContext<ProfileContext>();

        public MessageAPIController(PhotoAlbumsContext db)
        {
            this._db = db;
            //this._UserDb = userDb;
        }

        [HttpPost]
        public IActionResult SendMessage([FromBody] Message newMessage)
        {
            if (newMessage == null)
                return BadRequest();

            IdentityUser user = _db.Users.Where(user => user.Email == newMessage.Email).First();
            
            int success = 0;
            if(user != null)
            {
                newMessage.User= user;
                _db.Messages.Add(newMessage);
                success = _db.SaveChanges();
            }
            if (success != 1)
                return BadRequest();

            return Ok();
        }

    }
}
