using BadServer.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace BadServer.DataBase
{
    public class DbSeeder
    {
        private readonly MyDbContext _dbContext;

        public DbSeeder(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SeedAsync()
        {
            await _dbContext.Database.EnsureCreatedAsync();

            _dbContext.Imagenes.RemoveRange(_dbContext.Imagenes);
            _dbContext.Productos.RemoveRange(_dbContext.Productos);
            await _dbContext.SaveChangesAsync();

            await SeedImagesAsync();
            await SeedProductosAsync();
        }

        private async Task SeedImagesAsync()
        {
            Imagen[] images =
            {
                new Imagen() { ImagenNombre = "RaidenMgrImage", ImagenUrl = "images/raiden.jpg" },
                new Imagen() { ImagenNombre = "GarouOpmImage", ImagenUrl = "images/garou.jpg" }

            };



            await _dbContext.Imagenes.AddRangeAsync(images);
            await _dbContext.SaveChangesAsync();
        }

        private async Task SeedProductosAsync()
        {

            Imagen imagen1 = await _dbContext.Imagenes.FirstOrDefaultAsync(i => i.ImagenNombre == "RaidenMgrImage");
            Imagen imagen2 = await _dbContext.Imagenes.FirstOrDefaultAsync(i => i.ImagenNombre == "GarouOpmImage");

            Console.WriteLine($"Imagen1: {imagen1?.ImagenNombre}");
            Console.WriteLine($"Imagen2: {imagen2?.ImagenNombre}");

            Producto[] productos =
            {
            new Producto() { Cantidad = 10, Nombre = "Raiden Metal Gear Rising", Descripcion = "Mi espada es una herramienta de justicia", Precio = 19.99, Imagen = imagen1, Categoria = "Cyborg Ninja" },
            new Producto() { Cantidad = 5, Nombre = "Garou One Punch Man", Descripcion = "FIGHT OR BE FORGOTTEN!", Precio = 29.99, Imagen = imagen2, Categoria = "Demonio" }
    };

            await _dbContext.Productos.AddRangeAsync(productos);
            await _dbContext.SaveChangesAsync();
        }
    }
}
