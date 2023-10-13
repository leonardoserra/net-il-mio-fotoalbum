using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryAPIController : ControllerBase
    {

        private PhotoAlbumsContext _db = new PhotoAlbumsContext();

        public CategoryAPIController(PhotoAlbumsContext db)
        {
            this._db = db;
        }


        [HttpGet]
        public IActionResult GetCategories()
        {
            List<Category>? categories = _db.Categories.ToList();
            if (categories == null)
                return NotFound(new { message = "Nessuna categoria trovata." });

            return Ok(categories);
        }

        [HttpPost]
        public IActionResult Create([FromBody]Category newCategory)
        {
            if (newCategory == null )
                return BadRequest();

         
            _db.Categories.Add(newCategory);
            int success = _db.SaveChanges();

            if (success != 1)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest(new { message = "Inserire un id" });

            Photo? photoToDelete = _db.Photos.Where(photo => photo.Id == id)
                                    .FirstOrDefault();
            if (photoToDelete == null)
                return NotFound(new { message = "Foto non trovata a quell id" });

            _db.Remove(photoToDelete);
            int success = _db.SaveChanges();

            if (success != 1)
                return BadRequest(new { message = "Dati inviati non validi" });

            return Ok(photoToDelete);
        }

    }
}
