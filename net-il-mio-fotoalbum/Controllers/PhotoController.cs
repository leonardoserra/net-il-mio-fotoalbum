﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;
using System;
using System.Diagnostics;

namespace net_il_mio_fotoalbum.Controllers
{
    [Authorize(Roles = "ADMIN,USER")]
    public class PhotoController : Controller
    {
        protected PhotoAlbumsContext _db = new PhotoAlbumsContext();

        public PhotoController( PhotoAlbumsContext db)
        {
            _db = db;
        }


        [Authorize(Roles = "ADMIN,USER")]
        [HttpGet]
        public IActionResult Index()
        {
            List<Photo>? photos = new List<Photo>();
            try
            {
                if(User.IsInRole("ADMIN"))
                    photos = _db.Photos.Include(photo => photo.Categories).ToList<Photo>();
                else if (User.IsInRole("USER"))
                    photos = _db.Photos.Include(photo => photo.Categories).Where(photo=>photo.Visibility==true).ToList<Photo>();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Error");
            }
            return View("Index",photos);
        }


        [Authorize(Roles = "ADMIN,USER")]
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
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Create()
        {
            List<Category> categories = new List<Category>();
            List<SelectListItem> categoriesToSend = new List<SelectListItem>();
            PhotoComplex dataToSend;
            try
            {
                categories = _db.Categories.ToList();
                foreach(Category category in categories)
                {
                    categoriesToSend.Add(
                        new SelectListItem { Text = category.Title, Value = category.Id.ToString() }
                        );
                }
                
                dataToSend = new PhotoComplex {Photo=new Photo(),Categories=categoriesToSend };
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Error");
            }
            return View("Create", dataToSend);
        }

        [Authorize(Roles = "ADMIN")]
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
            if (dataReceived.SelectedCategoriesId != null)
            {
                foreach (string selectedCategory in dataReceived.SelectedCategoriesId) {
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
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Photo? photoToUpdate = _db.Photos.Include(photo=>photo.Categories).Where(photo=>photo.Id==id).FirstOrDefault();
            
            if(photoToUpdate == null)    
                return View("Error");
            
            List<Category> categories = new List<Category>();
            List<SelectListItem> categoriesToSend = new List<SelectListItem>();
            PhotoComplex dataToSend;
            try
            {
                categories = _db.Categories.ToList();
                foreach (Category category in categories)
                {
                    categoriesToSend.Add(
                        new SelectListItem { 
                            Text = category.Title,
                            Value = category.Id.ToString(),
                            Selected = photoToUpdate.Categories.Any(selectedCategory=> selectedCategory.Id == category.Id)                       
                        });
                }

                dataToSend = new PhotoComplex { Photo = photoToUpdate, Categories = categoriesToSend };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Error");
            }
            return View("Edit", dataToSend);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PhotoComplex dataReceived)
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
                            new SelectListItem { 
                                Text = category.Title,
                                Value = category.Id.ToString() }
                            );
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return View("Error");
                }

                dataReceived.Categories = categoriesToSend;
                
                return View("Edit", dataReceived);
            }

            //scrivo in db
            Photo? photoToUpdate = _db.Photos.Include(photo => photo.Categories).Where(photo => photo.Id == id).FirstOrDefault();

            if (photoToUpdate == null)
                return View("Error");
            
            photoToUpdate.Title = dataReceived.Photo.Title;
            photoToUpdate.Description = dataReceived.Photo.Description;

            if (dataReceived.ImageFile != null)
            {
                MemoryStream stream = new MemoryStream();
                dataReceived.ImageFile.CopyTo(stream);
                photoToUpdate.ImageFile = stream.ToArray();
            }


            photoToUpdate.Categories = new List<Category>();
            if (dataReceived.SelectedCategoriesId != null)
            {
                foreach (string selectedCategory in dataReceived.SelectedCategoriesId)
                {
                    int parsedCategoryId = int.Parse(selectedCategory);
                    Category? categoryInDb = _db.Categories.Where(category => category.Id == parsedCategoryId).FirstOrDefault();
                    if (categoryInDb != null)
                        photoToUpdate.Categories.Add(categoryInDb);
                }
            }
            //salva in db

            _db.SaveChanges();
            return RedirectToAction("Index");
        }



        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult SwitchVisibility(int id)
        {
            
            Photo? photo = _db.Photos.Where(photo => photo.Id == id).FirstOrDefault();
            if (photo != null)
                return View("SwitchVisibility", photo);
            
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SwitchVisibility(int id, Photo switchedVisibilityPhoto)
        {

            Photo? photo = _db.Photos.Where(photo => photo.Id == id).FirstOrDefault();
            if (photo != null)
            {
                photo.Visibility = switchedVisibilityPhoto.Visibility;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Photo? photoToDelete = _db.Photos.Where(photo => photo.Id == id).FirstOrDefault();
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
            if (formDataReceived.ImageFile == null)
            {
                return;
            }

            MemoryStream stream = new MemoryStream();
            formDataReceived.ImageFile.CopyTo(stream);
            formDataReceived.Photo.ImageFile = stream.ToArray();
        }
    }
}