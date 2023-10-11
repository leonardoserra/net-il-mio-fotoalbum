using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace net_il_mio_fotoalbum.Models
{
    [Table("categories")]
    public class Category
    {
        [Key]
        public int Id { get; set; }


        [Column("title")]
        [Required]
        [StringLength(100,ErrorMessage ="Massimo 100 caratteri per titolo categoria")]
        public string Title { get; set; }


        //relation n to n
        [Column("photos")]
        List<Photo>? Photos { get; set; }


        //constructor 
        public Category() { }   
    }
}
