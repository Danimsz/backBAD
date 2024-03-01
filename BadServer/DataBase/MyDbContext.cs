using BadServer.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace BadServer.DataBase
{
    public class MyDbContext : DbContext //Tiene que hederar de dbContenx
    {

        private const string DATABASE_PATH = "badinfluence.db";

        //Tablas 
        public DbSet<Cliente> Clientes { get; set; } 
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Cesta> Cestas { get; set; }
        public DbSet<CestaProducto> cestaProductos { get; set; }
        public DbSet<Imagen> Imagenes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoProducto> PedidoProductos { get; set; }


        //Configura Entity Framework para crear un archivo de base de dato sqlite
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
          string baseDir =  AppDomain.CurrentDomain.BaseDirectory;

          optionsBuilder.UseSqlite($"DataSource={baseDir}{DATABASE_PATH}");

        }

    }
}
