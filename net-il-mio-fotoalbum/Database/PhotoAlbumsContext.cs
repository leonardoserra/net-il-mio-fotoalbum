using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace net_il_mio_fotoalbum.Database
{
    public class PhotoAlbumsContext : IdentityDbContext<IdentityUser>
    {
        [Column("photos")]
        public DbSet<Photo> Photos { get; set; }

        [Column("categories")]
        public DbSet<Category> Categories { get; set; }

        [Column("messages")]
        public DbSet<Message> Messages { get; set; }

        //connection string to db
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=db-photo-albums;Integrated Security=True;TrustServerCertificate=True");
        }
    }
}
