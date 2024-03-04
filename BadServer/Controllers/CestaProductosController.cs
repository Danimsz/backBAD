﻿using BadServer.DataBase;
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

            //obtengo producto
            var producto = await _dbContext.Productos.FirstAsync(p => p.ProductoID == agregarProductoDto.ProductoID);

            if(producto.Cantidad >= agregarProductoDto.Cantidad)
            {
                //obtengo cesta
                var cestaProducto = await _dbContext.cestaProductos.FirstOrDefaultAsync(cp => cp.CestaID == cestaId && cp.ProductoID == agregarProductoDto.ProductoID);

                //Si el producto ya esta en la cesta
                if (cestaProducto != null)
                {
                    //se suma la cantidad indicada a la que ya habia
                    cestaProducto.Cantidad += agregarProductoDto.Cantidad;

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

                //restamos la cantidad de producto en la bd
                producto.Cantidad -= agregarProductoDto.Cantidad;

                //guardamos los cambios en la base de datos
                await _dbContext.SaveChangesAsync();
                return Ok(new { message = "El producto se ha agregado correctamente" });

            }
            else
            {
                return BadRequest("No hay stock de este producto");
            }
            
        }

        [HttpDelete("{cestaId}/quitar/{productoId}")]
        public async Task<IActionResult> QuitarProductoCesta(int cestaId, int productoId)
        {
            //obtengo producto
            var producto = await _dbContext.Productos.FirstAsync(p => p.ProductoID == productoId);

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

            //restamos la cantidad de producto en la bd
            producto.Cantidad += 1;

            await _dbContext.SaveChangesAsync();

            // Devolver la cantidad actualizada
            return Ok(new { cantidad = cestaProducto.Cantidad });
        }

        [HttpDelete("{cestaId}/limpiar")]
        public async Task<IActionResult> LimpiarCesta(int cestaId)
        {
            // Obtener todos los productos de la cesta
            var cestaProductos = await _dbContext.cestaProductos
                .Where(cp => cp.CestaID == cestaId)
                .ToListAsync();

            // Eliminar los productos de la cesta sin afectar el stock
            _dbContext.cestaProductos.RemoveRange(cestaProductos);
            await _dbContext.SaveChangesAsync();

            return Ok("Cesta limpiada exitosamente");
        }
    }
}

    
   
