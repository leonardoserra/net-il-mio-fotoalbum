using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class CategoryController : Controller
    {
        protected PhotoAlbumsContext _db = new PhotoAlbumsContext();

        public CategoryController(PhotoAlbumsContext db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Delete(int id)
        {
            Category? categoryToDelete = _db.Categories.Where(category => category.Id == id).FirstOrDefault();
            if (categoryToDelete == null)
                return View("Error");

            _db.Categories.Remove(categoryToDelete);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
