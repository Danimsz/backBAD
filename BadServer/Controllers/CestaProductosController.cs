using BadServer.DataBase;
using BadServer.DataBase.Dto;
using BadServer.DataBase.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BadServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class CestaProductosController : ControllerBase
    {
        private readonly MyDbContext _dbContext;

        public CestaProductosController(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{cestaId}/productos")]
        public async Task<IActionResult> VerProductosCesta(int cestaId)
        {
            var productos = await _dbContext.cestaProductos
                .Where(cp => cp.CestaID == cestaId)
                .Include(cp => cp.Producto)
                .Select(cp => new
                {
                    cp.Producto.Imagen,
                    cp.Producto.ProductoID,
                    cp.Producto.Nombre,
                    cp.Producto.Precio,
                    cp.Cantidad
                })
                .ToListAsync();

            return Ok(productos);
        }

        
        [HttpPost("{cestaId}/agregar")]
        public async Task<IActionResult> AgregarProductosCesta(int cestaId, [FromBody] AgregarProductoDto agregarProductoDto)
        {
            //Comprobamos si el producto existe en la tabla productos
            var productoExists = await _dbContext.Productos.AnyAsync(p => p.ProductoID == agregarProductoDto.ProductoID);
            if (!productoExists)
            {
                return BadRequest("El producto no existe");
            }

            //Añadimos el producto en la cesta
            var cestaProducto = await _dbContext.cestaProductos.FirstOrDefaultAsync(cp => cp.CestaID == cestaId && cp.ProductoID == agregarProductoDto.ProductoID);

            //Si el producto ya esta en la cesta, actualiza la cantidad de 1 en 1
            if (cestaProducto != null)
            {
                cestaProducto.Cantidad += 1;
            }
            else //si no esta en la cesta, lo añadimos
            {
                cestaProducto = new CestaProducto
                {
                    CestaID = cestaId,
                    ProductoID = agregarProductoDto.ProductoID,
                    Cantidad = agregarProductoDto.Cantidad
                };

                _dbContext.cestaProductos.Add(cestaProducto);
            }

            //guardamos los cambios en la base de datos
            await _dbContext.SaveChangesAsync();
            return Ok("El producto se ha agregado correctamente");
        }

        /*[HttpPut("{cestaId}/actualizar")]
        public async Task<IActionResult> ActualizarProductoCesta(int cestaId, [FromBody] AgregarProductoDto agregarProductoDto)
        {
            //Para ver si esta en la cesta el producto
            var productoExists = await _dbContext.cestaProductos.AnyAsync(cp => cp.CestaID == cestaId && cp.ProductoID == agregarProductoDto.ProductoID);
            if (!productoExists)
            {
                return BadRequest("El producto no se encuentra en la cesta");
            }

            //Si esta le cambia la cantidad
            var cestaProducto = await _dbContext.cestaProductos.FirstAsync(cp => cp.CestaID == cestaId && cp.ProductoID == agregarProductoDto.ProductoID);
            cestaProducto.Cantidad = agregarProductoDto.Cantidad;

            await _dbContext.SaveChangesAsync();

            return Ok("La cantidad del producto se ha actualizado correctamente");
        }*/

        [HttpDelete("{cestaId}/quitar/{productoId}")]
        public async Task<IActionResult> QuitarProductoCesta(int cestaId, int productoId)
        {
            var cestaProducto = await _dbContext.cestaProductos.FirstOrDefaultAsync(cp => cp.CestaID == cestaId && cp.ProductoID == productoId);
            if (cestaProducto == null)
            {
                return NotFound("El producto no se encuentra en la cesta");
            }

            if (cestaProducto.Cantidad > 1)
            {
                cestaProducto.Cantidad -= 1;
            }
            else
            {
                _dbContext.cestaProductos.Remove(cestaProducto);
            }

            await _dbContext.SaveChangesAsync();

            // Devolver la cantidad actualizada
            return Ok(new { cantidad = cestaProducto.Cantidad });
        }

    }
}
