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
            // Hashea la contrase�a proporcionada
            var hashedPassword = PasswordHelper.Hash(loginDto.Password);

            // Busca un usuario que coincida con el nombre de usuario y la contrase�a hasheada
            var user = await _dbContext.Clientes.Include(u => u.Cesta).FirstOrDefaultAsync(u => u.UserName == loginDto.UserName && u.Password == hashedPassword);

            // Control por si el usuario y la contrase�a no coinciden
            if (user == null)
            {
                return Unauthorized("Usuario o contrase�a incorrectos");
            }

            // Comprueba si el usuario tiene una cesta asociada
            if (user.Cesta == null)
            {
                return BadRequest("El usuario no tiene una cesta asociada");
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Aqui a�adimos los datos que sirvan para autorizar al usuario
                Claims = new Dictionary<string, object>
                {
                    { "id", Guid.NewGuid().ToString() },
                    { "CestaId", user.Cesta.CestaID.ToString() },
                    { "UserId", user.ClienteID }
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

            // Devolvemos tanto el token como el UserId y CestaId
            return Ok(new { Token = stringToken, UserId = user.ClienteID, CestaId = user.Cesta.CestaID, Rol = user.Rol });
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

            //hasheamos la contrase�a.
            //var hashedPassword = PasswordHelper.Hash(registerDto.Password);
            // Se crea el nuevo usuario, compruebalo bien Benito
            // Se crea el nuevo usuario
            var newUser = new Cliente
            {
                UserName = registerDto.UserName,
                //Hasheamos la contrase�a
                Password = PasswordHelper.Hash(registerDto.Password),
                Email = registerDto.Email,
                Address = registerDto.Address,
                Rol = "Usuario"
            };

            // A�ade el nuevo usuario a la base de datos
            _dbContext.Clientes.Add(newUser);
            await _dbContext.SaveChangesAsync();

            // Crea una nueva cesta para el usuario
            var newCesta = new Cesta();

            // Asigna la nueva cesta al usuario
            newUser.Cesta = newCesta;

            // A�ade la nueva cesta a la base de datos
            _dbContext.Cestas.Add(newCesta);
            await _dbContext.SaveChangesAsync();

            return Ok(new { UserId = newUser.ClienteID, CestaId = newCesta.CestaID });

        }
    }
}