using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace net_il_mio_fotoalbum.Models
{
    [Table("messages")]
    public class Message
    {
        [Key]
        public int Id { get; set; }


        [Column("text")]
        [Required(ErrorMessage = "Inserire il testo del messaggio")]
        [StringLength(2000, ErrorMessage = "Massimo 2000 caratteri per testo del messaggio")]
        public string Text { get; set; }


     
        //referring to the id of the AspNetUsers table to create relationship 1 to many 
        [Column("user")]
        public IdentityUser User { get; set; }

        //constructor 
        public Message() { }

    }
}
