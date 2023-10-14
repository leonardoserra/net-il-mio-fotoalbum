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
    }
}
