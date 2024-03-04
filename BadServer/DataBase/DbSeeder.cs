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

            if (!created)
            {
                if (!_dbContext.Productos.Any())
                {
                    await SeedImagesAsync();
                    await SeedProductosAsync();
                }
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
                new Imagen() { ImagenNombre = "12", ImagenUrl = "images/adidas_bom.jpg" },
                new Imagen() { ImagenNombre = "13", ImagenUrl = "images/adidas_camello.jpg" },
                new Imagen() { ImagenNombre = "14", ImagenUrl = "images/adidas_frikinav.jpg" },
                new Imagen() { ImagenNombre = "15", ImagenUrl = "images/adidas_hirmano.jpg" },
                new Imagen() { ImagenNombre = "16", ImagenUrl = "images/adidas_navaja.jpg" },
                new Imagen() { ImagenNombre = "17", ImagenUrl = "images/adidas_pato.jpg" },
                new Imagen() { ImagenNombre = "18", ImagenUrl = "images/jordan_email.jpg" },
                new Imagen() { ImagenNombre = "19", ImagenUrl = "images/jordan_x_kenkri.jpg" }

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
            Imagen imagen13 = await _dbContext.Imagenes.FirstOrDefaultAsync(i => i.ImagenNombre == "13");
            Imagen imagen14 = await _dbContext.Imagenes.FirstOrDefaultAsync(i => i.ImagenNombre == "14");
            Imagen imagen15 = await _dbContext.Imagenes.FirstOrDefaultAsync(i => i.ImagenNombre == "15");
            Imagen imagen16 = await _dbContext.Imagenes.FirstOrDefaultAsync(i => i.ImagenNombre == "16");
            Imagen imagen17 = await _dbContext.Imagenes.FirstOrDefaultAsync(i => i.ImagenNombre == "17");
            Imagen imagen18 = await _dbContext.Imagenes.FirstOrDefaultAsync(i => i.ImagenNombre == "18");
            Imagen imagen19 = await _dbContext.Imagenes.FirstOrDefaultAsync(i => i.ImagenNombre == "19");
            


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
            Console.WriteLine($"Imagen13: {imagen13?.ImagenNombre}");
            Console.WriteLine($"Imagen14: {imagen14?.ImagenNombre}");
            Console.WriteLine($"Imagen15: {imagen15?.ImagenNombre}");
            Console.WriteLine($"Imagen16: {imagen16?.ImagenNombre}");
            Console.WriteLine($"Imagen17: {imagen17?.ImagenNombre}");
            Console.WriteLine($"Imagen18: {imagen18?.ImagenNombre}");
            Console.WriteLine($"Imagen19: {imagen19?.ImagenNombre}");

            Producto[] productos =
            {
            new Producto() { Cantidad = 40, Nombre = "AIR FORCE BLUE", Descripcion = "AUR FORCE CLÁSICAS AZULES", Precio = 19.99, Imagen = imagen1, Categoria = "Air force" },
            new Producto() { Cantidad = 50, Nombre = "AIR FORCE CLOUD", Descripcion = "AIR FORCE CLOUD INSPIRADA EN LOS TONOS AZULADOS DEL CIELO", Precio = 29.99, Imagen = imagen2, Categoria = "Air force" },
            new Producto() { Cantidad = 100, Nombre = "AIR FORCE WHITE-OFF", Descripcion = "AIR FORCE X WHITE-OFF COLABORACION", Precio = 19.99, Imagen = imagen3, Categoria = "Air force" },
            new Producto() { Cantidad = 57, Nombre = "AIR FORCE VINTAGE", Descripcion = "AIR FORCE VINTAGE SIGUIENDO EL ESTILO DE LAS JORDAN 1", Precio = 29.99, Imagen = imagen4, Categoria = "Air force" },
            new Producto() { Cantidad = 108, Nombre = "JORDAN 1 BLUE", Descripcion = "JORDAN BLUE CLÁSICAS", Precio = 19.99, Imagen = imagen5, Categoria = "Jordan 1" },
            new Producto() { Cantidad = 56, Nombre = "JORDAN 1 WHITE-RED", Descripcion = "NO ME DA LA MENTE PARA INVENTAR MÁS DESCRIPCIONES", Precio = 29.99, Imagen = imagen6, Categoria = "Jordan 1" },
            new Producto() { Cantidad = 100, Nombre = "JORDAN 1 CLOUD", Descripcion = "JORDAN CLOUD INSPIRADAS EN LOS COLORES DE LA NATURALEZA", Precio = 19.99, Imagen = imagen7, Categoria = "Jordan 1" },
            new Producto() { Cantidad = 56, Nombre = "JORDAN 1 BROWN", Descripcion = "JORDAN CON ESTILO VINTAGE", Precio = 29.99, Imagen = imagen8, Categoria = "Jordan 1" },
            new Producto() { Cantidad = 100, Nombre = "JORDAN 1 BLACK LIMBO", Descripcion = "UN LIMBO DE COLOR NEGRO EN LAS NUEVAS JORDAN 1", Precio = 19.99, Imagen = imagen9, Categoria = "Jordan 1" },
            new Producto() { Cantidad = 58, Nombre = "JORDAN 1 PANDA", Descripcion = "NO COMEN BAMBU PERO CASI", Precio = 29.99, Imagen = imagen10, Categoria = "Jordan 1" },
            new Producto() { Cantidad = 100, Nombre = "JORDAN 1 RED", Descripcion = "JORDAN 1 MÍTICAS CON UN TOQUE ROJO", Precio = 19.99, Imagen = imagen11, Categoria = "Jordan 1" },
            new Producto() { Cantidad = 51, Nombre = "ADIDAS BOOM", Descripcion = "CUIDADO CON EL TEMPORIZADOR, VUELA EL EDIFICIO QUE MÁS TE MOLE", Precio = 100.99, Imagen = imagen12, Categoria = "Algeciras" },
            new Producto() { Cantidad = 50, Nombre = "ADIDAS CAMELLO", Descripcion = "HIRMANO CRUZA LA FRONTERA SIN MIEDO, METE AQUÍ LAS BELLOTAS DE POLEN PAKISTANÍ", Precio = 10.99, Imagen = imagen13, Categoria = "Algeciras" },
            new Producto() { Cantidad = 50, Nombre = "ADIDAS FRIKI NAVAJA", Descripcion = "ZAPATILLA QUE HACE COUNTER A LA ZAPATILLA NAVAJA, UN GITANO NO ESPERA UNA RESPUESTA ASÍ DE UN FRIKAZO", Precio = 55.99, Imagen = imagen14, Categoria = "Algeciras" },
            new Producto() { Cantidad = 59, Nombre = "ADIDAS HIRMANO", Descripcion = "CORRE DE LA POLICÍA HIRMANO MÁS RAPIDO, COLABORACIÓN CON MOHAMAD AL BIN RAPIDIHN", Precio = 60.99, Imagen = imagen15, Categoria = "Algeciras" },
            new Producto() { Cantidad = 56, Nombre = "ADIDAS NAVAJA", Descripcion = "MI ZAPATILLA ES UNA HERRAMIENTA PA ROBAH, NO HAY MEJOR OPCION SI ERES GITANO PREMO", Precio = 20.99, Imagen = imagen16, Categoria = "Algeciras" },
            new Producto() { Cantidad = 500, Nombre = "ADIDAS RECLAMO", Descripcion = "ADIDAS DESTINADAS A CAZAR, CON UN RECLAMO EN CADA CORDON Y UNO DE RESPUESTO PARA LLAMAR PATOS", Precio = 10.99, Imagen = imagen17, Categoria = "Algeciras" },
            new Producto() { Cantidad = 50, Nombre = "JORDAN EMAIL X BENITO", Descripcion = "COLABORACIÓN QUE REMEMORA LAS TRES SEMANAS QUE TARDO BENITO EN NO HACER EL EMAIL", Precio = 49.99, Imagen = imagen18, Categoria = "Algeciras" },
            new Producto() { Cantidad = 50, Nombre = "JORDAN X KENRKI GOD", Descripcion = "ESTAS ZAPATILLAS TE SUBEN DOS TONITOS!", Precio = 12.99, Imagen = imagen19, Categoria = "Algeciras" }
    };

            await _dbContext.Productos.AddRangeAsync(productos);
            await _dbContext.SaveChangesAsync();
        }
    }
}
