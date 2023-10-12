using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;
using System.Diagnostics;

namespace net_il_mio_fotoalbum.Controllers
{
    public class PhotoController : Controller
    {
        protected PhotoAlbumsContext _db = new PhotoAlbumsContext();

        public PhotoController( PhotoAlbumsContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Photo>? photos = new List<Photo>();
            try
            {
                photos = _db.Photos.Include(photo => photo.Categories).ToList<Photo>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Error");
            }
            return View("Index",photos);
        }
        [HttpGet]
        public IActionResult SearchPhotosByTitle(string? title)
        {
            List<Photo>? photos = new List<Photo>();
            try
            {
                if (title == null || title =="")
                {
                    photos = _db.Photos.Include(photo => photo.Categories).ToList<Photo>();
                }
                else
                {
                    photos = _db.Photos.Include(photo => photo.Categories).Where(photo=>photo.Title.ToLower().Contains(title.ToLower())).ToList<Photo>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Error");
            }
            return View("Index", photos);
        }



        [HttpGet]
        public IActionResult Details(int id)
        {
            Photo? photo = new Photo();
            try
            {
                photo = _db.Photos.Include(photo => photo.Categories).Where(photo => photo.Id == id).FirstOrDefault();
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
                return View("Error");
            }
            
            return View("Details", photo);
        }


        [HttpGet]
        public IActionResult Create() {

            return View("Create");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SwitchVisibility(int id)
        {
            /*Photo? photo = new Photo();
            try
            {
                photo = _db.Photos.Where(photo => photo.Id == id).FirstOrDefault();
                if(photo!=null)
                photo.Visibility = !photo.Visibility;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Error");
            }
            */
            return View("WorkInProgress");
        }










        public IActionResult Credits()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}