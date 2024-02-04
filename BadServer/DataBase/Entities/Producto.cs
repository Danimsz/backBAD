using System.Data.SqlTypes;

namespace BadServer.DataBase.Entities
{
    public class Producto
    {

        public int ProductoID { get; set; }
        public int Cantidad { get; set; }
        public string Modelo { get; set; }
        public string Descripcion { get; set; }
        public SqlMoney Precio { get; set; }
        public int Imagen { get; set; }
        public string UrlImagen { get; set; }


    }
}
