using BadServer.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace BadServer.DataBase
{
    public class MyDbContext : DbContext //Tiene que hederar de dbContenx
    {

        private const string DATABASE_PATH = "badinfluence.db";

        //Tablas 
        public DbSet<Cliente> Cliente { get; set; } //todos los clientes de     las tablas vienen aqui 


        //Configura Ef para crear un archivo de base de dato sqlite
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
          string baseDir =  AppDomain.CurrentDomain.BaseDirectory;

          optionsBuilder.UseSqlite($"DataSource={baseDir}{DATABASE_PATH}");

        }

    }
}
