using BadServer.DataBase;
using BadServer.DataBase.Dto;
using BadServer.DataBase.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BadServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginRegisterController : ControllerBase
    {
        private readonly MyDbContext _dbContext;

        public LoginRegisterController(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            
                // Aqui mira a ver si los datos del usuario son correctos para el incio de sesion
                var user = _dbContext.Cliente.FirstOrDefault(u => u.UserName == loginDto.UserName && u.Password == loginDto.Password);
                
                // Control por si el usuario y la contraseña no coinciden
                if (user == null)
                {
                    return Unauthorized("Usuario o contraseña incorrectos");
                }

               
                return Ok("Sesion iniciada correctamente");
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterDto registerDto)
        {
            
                // Aqui se comprueba si el usuario ya existe para no crearlo
                if (_dbContext.Cliente.Any(u => u.UserName == registerDto.UserName))
                {
                    return BadRequest("El nombre de usuario ya existe");
                }

                // Se crea el nuevo usuario, compruebalo bien Benito
                var newUser = new Cliente
                {
                    UserName = registerDto.UserName,
                    Password = registerDto.Password,
                    Email = registerDto.Email,
                    Address = registerDto.Address,
                    Rol = "Usuario" // Esto ultimo me dijo Amanda que lo añadiera
                };

                _dbContext.Cliente.Add(newUser);
                _dbContext.SaveChanges();

                return Ok("El usuario se ha registrado");
            
        }
    }
}
