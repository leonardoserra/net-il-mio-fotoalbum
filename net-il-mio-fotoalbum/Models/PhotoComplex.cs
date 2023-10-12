using Microsoft.AspNetCore.Mvc.Rendering;

namespace net_il_mio_fotoalbum.Models
{
    public class PhotoComplex
    {
        public Photo Photo { get; set; }

        public IFormFile? ImageFormFile { get; set; }
        public List<SelectListItem>? Categories { get; set; }
        public List<string> SelectedCategoryId { get; set; }



        public PhotoComplex() { }

    }
}
