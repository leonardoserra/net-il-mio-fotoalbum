using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace net_il_mio_fotoalbum.Models
{
    [Table("photos")]
    public class Photo
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }


        [Column("title")]
        [Required(ErrorMessage ="Il titolo è obbligatorio")]
        [StringLength(100, ErrorMessage="Massimo 100 caratteri nel titolo della foto.")]
        public string Title { get; set; }


        [Column("description")]
        [StringLength(500, ErrorMessage = "Massimo 500 caratteri nella descrizione della foto.")]
        [DefaultValue("Nessuna descrizione")]
        public string Description { get; set; } = "Nessuna descrizione";


        [Column("image_file")]
        public byte[]? ImageFile { get; set; }

        [Column("image_src")]
        public string ImageSrc => ImageFile is null ? "/img/default.png" : $"data:image/png;base64,{Convert.ToBase64String(ImageFile)}";


        [Column("visibility")]
        public bool Visibility { get; set; } = true;


        //relation n to n
        [Column("categories")]
        public List<Category>? Categories { get; set; }


        //constructor 
        public Photo() { }
    }
}
