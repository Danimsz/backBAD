using BadServer.DataBase;
using BadServer.DataBase.Dto;
using BadServer.DataBase.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace BadServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginRegisterController : ControllerBase
    {
        private readonly MyDbContext _dbContext;
        //Obtenemos por inyeccion los parametros preestablecidos para crear los token
        private readonly TokenValidationParameters _tokenParameters;


        public LoginRegisterController(IOptionsMonitor<JwtBearerOptions> jwtOptions, MyDbContext dbContext)
        {
            _dbContext = dbContext;
            _tokenParameters = jwtOptions.Get(JwtBearerDefaults.AuthenticationScheme)
                .TokenValidationParameters;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            // Hashea la contraseña proporcionada
            var hashedPassword = PasswordHelper.Hash(loginDto.Password);

            // Busca un usuario que coincida con el nombre de usuario y la contraseña hasheada
            var user = await _dbContext.Clientes.FirstOrDefaultAsync(u => u.UserName == loginDto.UserName && u.Password == hashedPassword);
              

            // Control por si el usuario y la contraseña no coinciden
            if (user == null)
            {
                return Unauthorized("Usuario o contraseña incorrectos");
            }

            // Asociar una cesta al usuario si no tiene una
            if (user.Cesta == null)
            {
                var existeCesta = await _dbContext.Cestas.FirstOrDefaultAsync(c => c.ClienteID == user.ClienteID);

                if (existeCesta == null)
                {
                    user.Cesta = new Cesta();
                    await _dbContext.SaveChangesAsync();

                    if (user.Cesta.CestaID > 0)
                    {
                        Console.WriteLine("La cesta se ha creado");
                    }
                    else
                    {
                        Console.WriteLine("La cesta NO se ha creado");
                    }
                } else
                {
                    Console.WriteLine("El usuario ya tiene una cesta");
                }

            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Aqui añadimos los datos que sirvan para autorizar al usuario
                Claims = new Dictionary<string, object>
            {
                { "id", Guid.NewGuid().ToString() },
                { "CestaId", user.Cesta.CestaID.ToString() },
                { "UserId", user.ClienteID } // Agregamos el UserId al token
            },
                //Aqui indicamos cuando caduca el token
                Expires = DateTime.UtcNow.AddDays(365),
                //Aqui especificamos nuestra clave y el algoritmo de firmado
                SigningCredentials = new SigningCredentials(_tokenParameters.IssuerSigningKey,
                SecurityAlgorithms.HmacSha256Signature)
            };

            //Creamos el token y se lo devolvemos al usuario logueado
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string stringToken = tokenHandler.WriteToken(token);

            // Devolvemos tanto el token como el UserId
            return Ok(new { Token = stringToken, UserId = user.ClienteID, CestaId = user.Cesta.CestaID });
        }



        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            
                // A benito le da error aqui
                // Aqui se comprueba si el usuario ya existe para no crearlo
                if (await _dbContext.Clientes.AnyAsync(u => u.UserName == registerDto.UserName))
                {
                    return BadRequest("El nombre de usuario ya existe");
                }

            //hasheamos la contraseña.
            //var hashedPassword = PasswordHelper.Hash(registerDto.Password);
            // Se crea el nuevo usuario, compruebalo bien Benito
            var newUser = new Cliente
                {
                    UserName = registerDto.UserName,
                    //Hasheamos la contraseña
                    Password = PasswordHelper.Hash(registerDto.Password),//registerDto.Password,
                    Email = registerDto.Email,
                    Address = registerDto.Address,
                    Rol = "Usuario" // Esto ultimo me dijo Amanda que lo añadiera
                };

                _dbContext.Clientes.Add(newUser);
                await _dbContext.SaveChangesAsync();

                return Ok("El usuario se ha registrado");
            
        }
        
    

    }
}
