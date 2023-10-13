using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        [Column("photos")]
        public List<Photo>? Photos { get; set; }


        //constructor 
        public Category() { }   
    }
}
