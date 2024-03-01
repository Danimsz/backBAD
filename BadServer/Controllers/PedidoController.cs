using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using BadServer.DataBase;
using BadServer.DataBase.Dto;
using BadServer.DataBase.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BadServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly MyDbContext _dbContext;

        public PedidoController(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("CrearPedido")]
        public async Task<IActionResult> CrearPedido([FromBody] PedidoDto pedidoDto)
        {
            try
            {
                Console.WriteLine($"Pedido recibido: {Newtonsoft.Json.JsonConvert.SerializeObject(pedidoDto)}");

                Cliente cliente = await _dbContext.Clientes.FindAsync(pedidoDto.ClienteID);

                Pedido nuevoPedido = new Pedido
                {
                    ClienteID = pedidoDto.ClienteID,
                    DireccionCliente = cliente.Address,
                    MetodoPago = pedidoDto.MetodoPago,
                    Total = pedidoDto.Total,
                    Estado = pedidoDto.Estado,
                    PrecioEuro = pedidoDto.PrecioEuro,
                    PrecioEtherium = pedidoDto.PrecioEtherium,
                    HashTransaccion = pedidoDto.HashTransaccion,
                    WalletCliente = pedidoDto.WalletCliente,
                    FechaPedido = pedidoDto.FechaPedido,
                };

                await _dbContext.Pedidos.AddAsync(nuevoPedido);
                await _dbContext.SaveChangesAsync();

                foreach (var productoDto in pedidoDto.Productos)
                {
                    if (productoDto.ProductoID <= 0)
                    {
                        return BadRequest("El ID del producto es inválido");
                    }

                    Producto producto = await _dbContext.Productos.FindAsync(productoDto.ProductoID);

                    if (producto != null)
                    {
                        PedidoProducto pedidoProducto = new PedidoProducto
                        {
                            PedidoID = nuevoPedido.Id,
                            ProductoID = productoDto.ProductoID,
                            Cantidad = productoDto.Cantidad,
                            PrecioUnitario = producto.Precio
                        };

                        await _dbContext.PedidoProductos.AddAsync(pedidoProducto);
                    }
                    else
                    {
                        return BadRequest($"No se encontró el producto con ID {productoDto.ProductoID}");
                    }
                }

                await _dbContext.SaveChangesAsync();

                return Ok(new { Message = "Pedido creado exitosamente" });
            }
            catch (Exception ex)
            {
                // Maneja cualquier excepción que pueda ocurrir durante el proceso
                return StatusCode(500, $"Error al procesar la solicitud: {ex.Message}");
            }
        }

        [HttpGet("VerPedido/{pedidoId}")]
        public async Task<IActionResult> ObtenerPedido(int pedidoId)
        {
            try
            {
                var pedido = await _dbContext.Pedidos
                    .Include(p => p.PedidoProductos)
                    .ThenInclude(pp => pp.Producto)
                    .FirstOrDefaultAsync(p => p.Id == pedidoId);

                if (pedido == null)
                {
                    return NotFound($"Pedido con ID {pedidoId} no encontrado");
                }

                var pedidoDto = new PedidoDetalleDto
                {
                    Id = pedidoId,
                    ClienteID = pedido.ClienteID,
                    MetodoPago = pedido.MetodoPago,
                    Total = pedido.Total,
                    Estado = pedido.Estado,
                    PrecioEuro = pedido.PrecioEuro,
                    PrecioEtherium = pedido.PrecioEtherium,
                    HashTransaccion = pedido.HashTransaccion,
                    WalletCliente = pedido.WalletCliente,
                    FechaPedido = pedido.FechaPedido,
                    Productos = pedido.PedidoProductos.Select(pp => new DetallesProductoDto
                    {
                        ProductoID = pp.ProductoID,
                        Cantidad = pp.Cantidad,
                        Nombre = pp.Producto.Nombre,
                        Descripcion = pp.Producto.Descripcion,
                        Precio = pp.PrecioUnitario,
                        ImagenID = pp.Producto.Imagen?.ImagenID,
                        ImagenNombre = pp.Producto.Imagen?.ImagenNombre,
                        ImagenUrl = pp.Producto.Imagen?.ImagenUrl,
                        Categoria = pp.Producto.Categoria
                    }).ToList()
                };

                return Ok(pedidoDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al procesar la solicitud: {ex.Message}");
            }
        }


        [HttpGet("VerPedidos")]
        public async Task<IActionResult> ObtenerTodosPedidos()
        {
            try
            {
                var pedidos = await _dbContext.Pedidos
                    .Include(p => p.PedidoProductos)
                    .ThenInclude(pp => pp.Producto)
                    .ToListAsync();

                var pedidosDto = pedidos.Select(pedido => new PedidoDetalleDto
                {
                    Id = pedido.Id,
                    ClienteID = pedido.ClienteID,
                    MetodoPago = pedido.MetodoPago,
                    Total = pedido.Total,
                    Estado = pedido.Estado,
                    PrecioEuro = pedido.PrecioEuro,
                    PrecioEtherium = pedido.PrecioEtherium,
                    HashTransaccion = pedido.HashTransaccion,
                    WalletCliente = pedido.WalletCliente,
                    FechaPedido = pedido.FechaPedido,
                    Productos = pedido.PedidoProductos.Select(pp => new DetallesProductoDto
                    {
                        ProductoID = pp.ProductoID,
                        Cantidad = pp.Cantidad,
                        Nombre = pp.Producto.Nombre,
                        Descripcion = pp.Producto.Descripcion,
                        Precio = pp.PrecioUnitario,
                        ImagenID = pp.Producto.Imagen?.ImagenID,
                        ImagenNombre = pp.Producto.Imagen?.ImagenNombre,
                        ImagenUrl = pp.Producto.Imagen?.ImagenUrl,
                        Categoria = pp.Producto.Categoria
                    }).ToList()
                }).ToList();

                return Ok(pedidosDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al procesar la solicitud: {ex.Message}");
            }
        }

        [HttpPut("EditarPedido/{pedidoId}")]
        public async Task<IActionResult> EditarPedido(int pedidoId, [FromBody] PedidoDto pedidoDto)
        {
            try
            {
                var pedido = await _dbContext.Pedidos
                .Include(p => p.PedidoProductos)
                .FirstOrDefaultAsync(p => p.Id == pedidoId);

                if (pedido == null)
                {
                    return NotFound($"Pedido con ID {pedidoId} no encontrado");
                }
                
                pedido.MetodoPago = pedidoDto.MetodoPago;
                pedido.Total = pedidoDto.Total;
                pedido.Estado = pedidoDto.Estado;
                pedido.PrecioEuro = pedidoDto.PrecioEuro;
                pedido.PrecioEtherium = pedidoDto.PrecioEtherium;
                pedido.HashTransaccion = pedidoDto.HashTransaccion;
                pedido.WalletCliente = pedidoDto.WalletCliente;
                pedido.FechaPedido = pedidoDto.FechaPedido;

                
                foreach (var detalleDto in pedidoDto.Productos)
                {
                    var detallePedido = pedido.PedidoProductos.FirstOrDefault(pp => pp.ProductoID == detalleDto.ProductoID);

                    if (detallePedido != null)
                    {
                        detallePedido.Cantidad = detalleDto.Cantidad;
                        detallePedido.PrecioUnitario = detalleDto.PrecioUnitario;
                    }
                }

                await _dbContext.SaveChangesAsync();

                return Ok($"Pedido con ID {pedidoId} editado correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al procesar la solicitud: {ex.Message}");
            }
        }

        [HttpDelete("BorrarPedido/{pedidoId}")]
        public async Task<IActionResult> BorrarPedido(int pedidoId)
        {
            try
            {
                var pedido = await _dbContext.Pedidos.FindAsync(pedidoId);

                if (pedido == null)
                {
                    return NotFound($"Pedido con ID {pedidoId} no encontrado");
                }

                _dbContext.Pedidos.Remove(pedido);
                await _dbContext.SaveChangesAsync();

                return Ok($"Pedido con ID {pedidoId} borrado correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al procesar la solicitud: {ex.Message}");
            }
        }
    }
}

