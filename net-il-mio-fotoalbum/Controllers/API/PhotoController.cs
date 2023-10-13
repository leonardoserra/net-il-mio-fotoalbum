using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {

        private PhotoAlbumsContext _db = new PhotoAlbumsContext();

        public PhotoController(PhotoAlbumsContext db)
        {
            this._db = db;
        }

       
        [HttpGet]
        public IActionResult GetPhotos()
        {
            List<Photo>? photos = _db.Photos.Include(photo => photo.Categories).ToList();
            if (photos == null)
                return NotFound(new { message = "Nessuna foto trovata." });

            return Ok(photos);
        }
        [HttpGet]
        public IActionResult SearchPhotoByTitle(string? search)
        {

            if (search == null || search.Length == 0)
                return this.GetPhotos();

            List<Photo>? photos = _db.Photos.Include(photo => photo.Categories)
                                .Where(photo => photo.Title.ToLower()
                                .Contains(search.ToLower()))
                                .ToList();
            if (photos == null)
                return NotFound(new { message = "Nessuna foto trovata." });

            return Ok(photos);
        }

    }
}
