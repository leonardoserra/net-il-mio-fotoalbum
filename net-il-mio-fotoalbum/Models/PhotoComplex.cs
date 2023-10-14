using Microsoft.AspNetCore.Mvc.Rendering;

namespace net_il_mio_fotoalbum.Models
{
    public class PhotoComplex
    {
        public Photo Photo { get; set; }

        public IFormFile? ImageFile { get; set; }
        public List<SelectListItem>? Categories { get; set; }
        public List<string>? SelectedCategoriesId { get; set; }



        public PhotoComplex() { }

    }
}
