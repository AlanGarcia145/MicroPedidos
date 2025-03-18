using MicroAlmacen.Modelo;
using Microsoft.EntityFrameworkCore;

namespace MicroAlmacen.Persistencia
{
    public class ContextoAlmacen : DbContext
    {
        public ContextoAlmacen(DbContextOptions<ContextoAlmacen> options) : base(options)
        {

        }

        public DbSet<Almacen> Almacen { get; set; }

        public DbSet<Pedido> Pedido { get; set; }

        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("user");

            modelBuilder.Entity<Pedido>().ToTable("pedido");
        }
    }
}
