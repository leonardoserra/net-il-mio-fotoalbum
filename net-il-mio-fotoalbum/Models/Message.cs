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
        [Required]
        [StringLength(2000, ErrorMessage = "Massimo 2000 caratteri per testo del messaggio")]
        public string Text { get; set; }


        /*//relation n to 1
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("user")]
        public User User { get; set; }*/


        //constructor 
        public Message() { }
    }
}
