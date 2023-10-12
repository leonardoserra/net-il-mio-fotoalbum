using Microsoft.AspNetCore.Authorization;
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



        //TO DO
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
        public IActionResult Create(PhotoComplex dataReceived)
        {
            if (!ModelState.IsValid)
            {
                
                List<Category> categories = new List<Category>();
                List<SelectListItem> categoriesToSend = new List<SelectListItem>();
                try
                {
                    categories = _db.Categories.ToList();
                    foreach (Category category in categories)
                    {
                        categoriesToSend.Add(
                            new SelectListItem { Text = category.Title, Value = category.Id.ToString() }
                            );
                    }

                }catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return View("Error");
                }


                dataReceived.Categories = categoriesToSend;
                return View("Create", dataReceived);
            }

            dataReceived.Photo.Categories = new List<Category>();
            if (dataReceived.SelectedCategoryId != null)
            {
                foreach (string selectedCategory in dataReceived.SelectedCategoryId) {
                    int parsedCategoryId = int.Parse(selectedCategory);
                    Category? categoryInDb = _db.Categories.Where(category => category.Id == parsedCategoryId).FirstOrDefault();
                    if (categoryInDb != null)
                        dataReceived.Photo.Categories.Add(categoryInDb);
                }
            }
            this.SetImageFileFromFormFile(dataReceived);

            //salva in db
            _db.Photos.Add(dataReceived.Photo);
            _db.SaveChanges();
            return RedirectToAction("Index");
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Photo? photoToDelete = _db.Photos.Where(pizza => pizza.Id == id).FirstOrDefault();
            if (photoToDelete == null)
                return View("Error");

            _db.Photos.Remove(photoToDelete);
            _db.SaveChanges();

            return RedirectToAction("Index");
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

        //Funzione che converte il file mandato dal form in byte[] e imposta l'immagine
        private void SetImageFileFromFormFile(PhotoComplex formDataReceived)
        {
            if (formDataReceived.ImageFormFile == null)
            {
                return;
            }

            MemoryStream stream = new MemoryStream();
            formDataReceived.ImageFormFile.CopyTo(stream);
            formDataReceived.Photo.ImageFile = stream.ToArray();
        }
    }
}