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
    [Authorize(Roles = "ADMIN,USER")]
    [ApiController]
    public class PhotoAPIController : ControllerBase
    {

        private PhotoAlbumsContext _db = new PhotoAlbumsContext();

        public PhotoAPIController(PhotoAlbumsContext db)
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
        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult SwitchVisibility(int id)
        {
            try
            {
                Photo? photo = _db.Photos.Where(photo => photo.Id == id).FirstOrDefault();
                if (photo != null)
                {
                    if (photo.Visibility)
                        photo.Visibility = false;
                    else
                        photo.Visibility = true;
                }
                    
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Redirect("/Error");
            }

            return Redirect("/Photo/Index");
        }
    }
}
