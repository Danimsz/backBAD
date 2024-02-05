namespace BadServer.DataBase.Entities
{
    public class Cesta
    {
        public int CestaID { get; set; }
        public int ClienteID { get; set; }
        public ICollection<CestaProducto> CestaProductos { get; set; }
    }
}
