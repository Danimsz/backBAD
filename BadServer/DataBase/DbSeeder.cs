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
            bool created = await _dbContext.Database.EnsureCreatedAsync();
            
            if (!created) {
            await SeedImagesAsync();
            await SeedProductosAsync();
            await _dbContext.SaveChangesAsync();
            }
        }

        private async Task SeedImagesAsync()
        {
            Imagen[] images =
            {
                new Imagen() { ImagenNombre = "1", ImagenUrl = "images/airforce_azul.jpg" },
                new Imagen() { ImagenNombre = "2", ImagenUrl = "images/airforce_blancaceleste.jpg" },
                new Imagen() { ImagenNombre = "3", ImagenUrl = "images/airforce_blancas.jpg" },
                new Imagen() { ImagenNombre = "4", ImagenUrl = "images/airforce_negracrudo.jpg" },
                new Imagen() { ImagenNombre = "5", ImagenUrl = "images/jordan1_azul.jpg" },
                new Imagen() { ImagenNombre = "6", ImagenUrl = "images/jordan1_blancarojo.jpg" },
                new Imagen() { ImagenNombre = "7", ImagenUrl = "images/jordan1_celetes.jpg" },
                new Imagen() { ImagenNombre = "8", ImagenUrl = "images/jordan1_marron.jpg" },
                new Imagen() { ImagenNombre = "9", ImagenUrl = "images/jordan1_negrogris.jpg" },
                new Imagen() { ImagenNombre = "10", ImagenUrl = "images/jordan1_panda.jpg" },
                new Imagen() { ImagenNombre = "11", ImagenUrl = "images/jordan1_roja.jpg" },
                new Imagen() { ImagenNombre = "12", ImagenUrl = "images/jordan1_rojablanca.jpg" }

            };



            await _dbContext.Imagenes.AddRangeAsync(images);
            await _dbContext.SaveChangesAsync();
        }

        private async Task SeedProductosAsync()
        {

            Imagen imagen1 = await _dbContext.Imagenes.FirstOrDefaultAsync(i => i.ImagenNombre == "1");
            Imagen imagen2 = await _dbContext.Imagenes.FirstOrDefaultAsync(i => i.ImagenNombre == "2");
            Imagen imagen3 = await _dbContext.Imagenes.FirstOrDefaultAsync(i => i.ImagenNombre == "3");
            Imagen imagen4 = await _dbContext.Imagenes.FirstOrDefaultAsync(i => i.ImagenNombre == "4");
            Imagen imagen5 = await _dbContext.Imagenes.FirstOrDefaultAsync(i => i.ImagenNombre == "5");
            Imagen imagen6 = await _dbContext.Imagenes.FirstOrDefaultAsync(i => i.ImagenNombre == "6");
            Imagen imagen7 = await _dbContext.Imagenes.FirstOrDefaultAsync(i => i.ImagenNombre == "7");
            Imagen imagen8 = await _dbContext.Imagenes.FirstOrDefaultAsync(i => i.ImagenNombre == "8");
            Imagen imagen9 = await _dbContext.Imagenes.FirstOrDefaultAsync(i => i.ImagenNombre == "9");
            Imagen imagen10 = await _dbContext.Imagenes.FirstOrDefaultAsync(i => i.ImagenNombre == "10");
            Imagen imagen11 = await _dbContext.Imagenes.FirstOrDefaultAsync(i => i.ImagenNombre == "11");
            Imagen imagen12 = await _dbContext.Imagenes.FirstOrDefaultAsync(i => i.ImagenNombre == "12");
            

            Console.WriteLine($"Imagen1: {imagen1?.ImagenNombre}");
            Console.WriteLine($"Imagen2: {imagen2?.ImagenNombre}");
            Console.WriteLine($"Imagen3: {imagen3?.ImagenNombre}");
            Console.WriteLine($"Imagen4: {imagen4?.ImagenNombre}");
            Console.WriteLine($"Imagen5: {imagen5?.ImagenNombre}");
            Console.WriteLine($"Imagen6: {imagen6?.ImagenNombre}");
            Console.WriteLine($"Imagen7: {imagen7?.ImagenNombre}");
            Console.WriteLine($"Imagen8: {imagen8?.ImagenNombre}");
            Console.WriteLine($"Imagen9: {imagen9?.ImagenNombre}");
            Console.WriteLine($"Imagen10: {imagen10?.ImagenNombre}");
            Console.WriteLine($"Imagen11: {imagen11?.ImagenNombre}");
            Console.WriteLine($"Imagen12: {imagen12?.ImagenNombre}");

            Producto[] productos =
            {
            new Producto() { Cantidad = 10, Nombre = "AIR FORCE BLUE", Descripcion = "Mi espada es una herramienta de justiciaaaaaaaaaa", Precio = 19.99, Imagen = imagen1, Categoria = "Air force" },
            new Producto() { Cantidad = 5, Nombre = "AIR FORCE CLOUD", Descripcion = "FIGHT OR BE FORGOTTEN!", Precio = 29.99, Imagen = imagen2, Categoria = "Air force" },
            new Producto() { Cantidad = 10, Nombre = "AIR FORCE WHITE-OFF", Descripcion = "Mi espada es una herramienta de justiciaaaaaaaaaaaaaa", Precio = 19.99, Imagen = imagen3, Categoria = "Air force" },
            new Producto() { Cantidad = 5, Nombre = "AIR FORCE VINTAGE", Descripcion = "FIGHT OR BE FORGOTTEN!", Precio = 29.99, Imagen = imagen4, Categoria = "Air force" },
            new Producto() { Cantidad = 10, Nombre = "JORDAN 1 BLUE", Descripcion = "Mi espada es una herramienta de justicia", Precio = 19.99, Imagen = imagen5, Categoria = "Jordan 1" },
            new Producto() { Cantidad = 5, Nombre = "JORDAN 1 WHITE-RED", Descripcion = "FIGHT OR BE FORGOTTEN!", Precio = 29.99, Imagen = imagen6, Categoria = "Jordan 1" },
            new Producto() { Cantidad = 10, Nombre = "JORDAN 1 CLOUD", Descripcion = "Mi espada es una herramienta de justicia", Precio = 19.99, Imagen = imagen7, Categoria = "Jordan 1" },
            new Producto() { Cantidad = 5, Nombre = "JORDAN 1 BROWN", Descripcion = "FIGHT OR BE FORGOTTEN!", Precio = 29.99, Imagen = imagen8, Categoria = "Jordan 1" },
            new Producto() { Cantidad = 10, Nombre = "JORDAN 1 BLACK LIMBO", Descripcion = "Mi espada es una herramienta de justicia", Precio = 19.99, Imagen = imagen9, Categoria = "Jordan 1" },
            new Producto() { Cantidad = 5, Nombre = "JORDAN 1 PANDA", Descripcion = "FIGHT OR BE FORGOTTEN!", Precio = 29.99, Imagen = imagen10, Categoria = "Jordan 1" },
            new Producto() { Cantidad = 10, Nombre = "JORDAN 1 RED", Descripcion = "Mi espada es una herramienta de justicia", Precio = 19.99, Imagen = imagen11, Categoria = "Jordan 1" },
            new Producto() { Cantidad = 5, Nombre = "JORDAN BLOOD", Descripcion = "FIGHT OR BE FORGOTTEN!", Precio = 29.99, Imagen = imagen12, Categoria = "Jordan 1" }
    };

            await _dbContext.Productos.AddRangeAsync(productos);
            await _dbContext.SaveChangesAsync();
        }
    }
}
