using BadServer.DataBase;
using BadServer.DataBase.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BadServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly MyDbContext _dbContext;

        public UsuarioController(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("VerUsuarios")]
        public async Task<IActionResult> VerUsuarios()
        {
            try
            {
                var usuarios = await _dbContext.Clientes
                    .Select(u => new
                    {
                        u.ClienteID,
                        u.UserName,
                        u.Rol,
                        u.Email
                    })
                    .ToListAsync();

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al procesar la solicitud: {ex.Message}");
            }
        }

        [HttpGet("VerUsuario/{usuarioId}")]
        public async Task<IActionResult> VerUsuario(int usuarioId)
        {
            try
            {
                var usuario = await _dbContext.Clientes
                    .Where(u => u.ClienteID == usuarioId)
                    .Select(u => new
                    {
                        u.ClienteID,
                        u.UserName,
                        u.Rol,
                        u.Email
                    })
                    .FirstOrDefaultAsync();

                if (usuario == null)
                {
                    return NotFound($"Usuario con ID {usuarioId} no encontrado");
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al procesar la solicitud: {ex.Message}");
            }
        }

        [HttpPut("EditarUsuarioAdmin/{usuarioId}")]
        public async Task<IActionResult> EditarUsuario(int usuarioId, [FromBody] UsuarioEditAdminDto usuarioDto)
        {
            try
            {
                var usuario = await _dbContext.Clientes.FindAsync(usuarioId);

                if (usuario == null)
                {
                    return NotFound($"Usuario con ID {usuarioId} no encontrado");
                }

                usuario.Rol = usuarioDto.Rol;
                usuario.Email = usuarioDto.Email;

                await _dbContext.SaveChangesAsync();

                return Ok($"Usuario con ID {usuarioId} editado correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al procesar la solicitud: {ex.Message}");
            }
        }

        [HttpPut("EditarUsuario/{usuarioId}")]
        public async Task<IActionResult> EditarUsuario(int usuarioId, [FromBody] EditarUsuarioDto editarUsuarioDto)
        {
            try
            {
                var usuario = await _dbContext.Clientes.FirstOrDefaultAsync(u => u.ClienteID == usuarioId);

                if (usuario == null)
                {
                    return NotFound($"Usuario con ID {usuarioId} no encontrado");
                }

                usuario.UserName = editarUsuarioDto.UserName ?? usuario.UserName;
                usuario.Email = editarUsuarioDto.Email ?? usuario.Email;
                usuario.Address = editarUsuarioDto.Address ?? usuario.Address;

                if (!string.IsNullOrEmpty(editarUsuarioDto.Password))
                {
                    usuario.Password = PasswordHelper.Hash(editarUsuarioDto.Password);
                }

                await _dbContext.SaveChangesAsync();

                return Ok(new
                {
                    usuario.ClienteID,
                    usuario.UserName,
                    usuario.Email,
                    usuario.Address
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al procesar la solicitud: {ex.Message}");
            }
        }


        [HttpDelete("BorrarUsuario/{usuarioId}")]
        public async Task<IActionResult> BorrarUsuario(int usuarioId)
        {
            try
            {
                var usuario = await _dbContext.Clientes.FindAsync(usuarioId);

                if (usuario == null)
                {
                    return NotFound($"Usuario con ID {usuarioId} no encontrado");
                }

                _dbContext.Clientes.Remove(usuario);
                await _dbContext.SaveChangesAsync();

                return Ok($"Usuario con ID {usuarioId} borrado correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al procesar la solicitud: {ex.Message}");
            }
        }

    }
}
