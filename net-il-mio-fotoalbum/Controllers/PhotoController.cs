using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        [HttpPost]
        /*[Route("/Photo/SearchPhotosByTitle/{searchString}", Name = "SearchPhotosByTitle")]*/
        [ValidateAntiForgeryToken]
        public IActionResult SearchPhotosByTitle(string? searchString)
        {
            List<Photo>? photos = new List<Photo>();
            try
            {
                if (searchString == null || searchString =="")
                {
                    return this.Index();
                    /*photos = _db.Photos.Include(photo => photo.Categories).ToList<Photo>();*/
                }
                else
                {
                    photos = _db.Photos.Include(photo => photo.Categories).Where(photo=>photo.Title.ToLower().Contains(searchString.ToLower())).ToList<Photo>();
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


        /*CREATE*/
        [HttpGet]
        public IActionResult Create()
        {
            List<Category> categories = new List<Category>();
            List<SelectListItem> categoriesToSend = new List<SelectListItem>();
            PhotoComplex photoComplex;
            try
            {
                categories = _db.Categories.ToList();
                foreach(Category category in categories)
                {
                    categoriesToSend.Add(
                        new SelectListItem { Text = category.Title, Value = category.Id.ToString() }
                        );
                }
                
                photoComplex = new PhotoComplex {Photo=new Photo(),Categories=categoriesToSend };
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Error");
            }
            return View("Create", photoComplex);
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