using BadServer.DataBase;
using BadServer.DataBase.Dto;
using BadServer.DataBase.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            return Ok("Sesion iniciada correctamente");
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
