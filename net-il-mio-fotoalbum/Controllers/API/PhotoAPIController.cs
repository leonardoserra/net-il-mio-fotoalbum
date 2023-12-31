﻿using Microsoft.AspNetCore.Authorization;
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
            List<Photo>? photos = new List<Photo>();
            if (User.IsInRole("ADMIN"))
            {
                photos = _db.Photos.Include(photo => photo.Categories).ToList<Photo>();
            }
            else if (User.IsInRole("USER"))
            {
                photos = _db.Photos.Include(photo => photo.Categories).Where(photo => photo.Visibility == true).ToList<Photo>();
            }

            if (photos == null || photos.Count == 0)
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
