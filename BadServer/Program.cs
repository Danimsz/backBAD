
using BadServer.DataBase;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BadServer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //añade la dependencia de los controladores

            builder.Services.AddControllers();

            //Habilitamos CORS para enlazar front con back en localhost
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddCors(options =>
                {
                    options.AddDefaultPolicy(builder =>
                    {
                        builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
                });
            }

            //Agrega el servicio API Explorer al proyecto.
            //Esto permite explorar y documentar los puntos finales
            //(los endpoints) de la API de una manera más sencilla
            //también es utilizado por Swagger/OpenAPI  
            // Más info https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();///swagger es un estandar que da una interfaz grafica en la pagina web que de manera grafica puedes probar tus distintos endpoint

            builder.Services.AddScoped<MyDbContext>();

            //añadimos el dbcontext al servicio de ingeccion de indepemdencia
            //Tiene que ser scope para que cierre la conexion y limpie
            //Los recursos tras cada peticion
            builder.Services.AddScoped<MyDbContext>();
            builder.Services.AddTransient<DbSeeder>();

            builder.Services.AddAuthentication()
             .AddJwtBearer(options =>
             {
                 //Por seguirdad guardamos la clave privada en la variable de entorno
                 ////La clave debe tener mas de 256 bits
                 string key = Environment.GetEnvironmentVariable("JWT_KEY");

                 options.TokenValidationParameters = new TokenValidationParameters()
                 {

                     //Si no nos importa que se valide el emisor del token, lo desactivamos
                     ValidateIssuer = false,
                     ///Si no nos imporata que se valida para quien o 
                     ///para que proposito esta destinado el token,lo desactivamos 
                     ValidateAudience = false,
                     //Indicamos la clave
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                 };
             });

            var app = builder.Build();//construyo la aplicaicon 

            //Creamos un scope y nos aseguramos de que se crea la base de dato
            //Segun tengamos configurado nuestro dbContentx
            //Pagina 38 Pdf ASP.NET y nos falta los endpoint del controller
            using (IServiceScope scope = app.Services.CreateScope())
                
            {
                MyDbContext dbContext = scope.ServiceProvider.GetService<MyDbContext>();
               dbContext.Database.EnsureCreated();

                DbSeeder dbSeeder = scope.ServiceProvider.GetService<DbSeeder>();

                if (dbSeeder != null)
                {
                    await dbSeeder.SeedAsync();
                }
                else
                {
                    Console.WriteLine("El servicio DbSeeder es nulo.");
                }

            }

            // Verifica si la aplicación se está ejecutando.
            // en un entorno de desarollo. Si es así se activa
            // la visualización de Swagger y su interfaz de usuario
            // (la pagina esta donde pone get y tal para hacer las llamadas)
            // En un entorno de producción, esta sección en principio se omite
            // la documentación de la API publicamente.

            //si la aplicacion esta en una version desarrollo quiero que se meustre el swagger
            //pq una version publica en produccion das informacion y la pueden ver 
            //Por eso viene de forma predeterminada en este if
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                //Permite CORS
                app.UseCors();
            }


            //Configura la redireccion HTTP a HTTPS
            //Esto redirige todas las solicitudes HTTP
            // a sus  equivalente HTTPS para mejorar la seguridad

            //configura la redireccion de hhtp a https
            app.UseHttpsRedirection();

            //Configura el middleware de autorizacion
            // Lo que indica que la aplicacion esta habilitada
            //Para usar sistemas de autenticaciom y autorizacion

            //habilita la autenticacion
            app.UseAuthentication();
         
            //habilita la autiorizacion
            app.UseAuthorization();

            //Configura la aplicacion para que utilize
            //Los controladores que se registraon anterionmente
            //PAra manejar las solicitudes  HTTP entrantes 
            app.MapControllers();

            app.Run();
        }
    }
}
