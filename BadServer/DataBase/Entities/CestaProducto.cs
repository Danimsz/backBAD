namespace BadServer.DataBase.Entities
{
    public class CestaProducto
    {   
        public int CestaProductoID { get; set; }
        public int CestaID { get; set; }
        public Cesta Cesta { get; set; }
        public int ProductoID { get; set; }
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
    }
}
