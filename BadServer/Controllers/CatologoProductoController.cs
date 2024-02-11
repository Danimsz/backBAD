using BadServer.DataBase;
using BadServer.DataBase.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BadServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatologoProductoController : ControllerBase
    {
        private readonly MyDbContext _dbContext;
        public CatologoProductoController(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("Catalogo")]
        public async Task<IActionResult> Catalogo()
        {
            try
            {
                var productos = await _dbContext.Productos
                    .Select(p => new CatalogoDto
                    {
                        ProductoID = p.ProductoID,
                        Nombre = p.Nombre,
                        Precio = p.Precio,
                        Imagen = p.Imagen
                    })
                    .ToListAsync();

                return Ok(productos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener el catálogo: {ex.Message}");
            }
        }
    }
}
