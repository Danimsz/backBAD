namespace BadServer.DataBase.Entities
{
    public class PedidoProducto
    {
        public int PedidoProductoID { get; set; }
        public int PedidoID { get; set; }
        public Pedido Pedido { get; set; }

        public int ProductoID { get; set; }
        public Producto Producto { get; set; }

        public int Cantidad { get; set; }
        public double PrecioUnitario { get; set; }
    }
}

