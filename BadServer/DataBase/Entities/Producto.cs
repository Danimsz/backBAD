

namespace BadServer.DataBase.Entities
{
    public class Producto
    {

        public int ProductoID { get; set; }
        public int Cantidad { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public double Precio { get; set; }
        public int? ImagenID { get; set; }
        public Imagen Imagen { get; set; }
        public string Categoria { get; set; }   
        public ICollection<CestaProducto> CestaProductos { get; set; }
        public ICollection<PedidoProducto> PedidoProductos { get; set; }
    }
}
