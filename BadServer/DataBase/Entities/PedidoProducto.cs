using System.Data.SqlTypes;

namespace BadServer.DataBase.Entities
{
    public class PedidoProducto
    {

        public int PedidoProductoID { get; set; }
        public int PedidoID { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }

        //SqlMoney es un tipo de dato específico de SQL Server que se utiliza para almacenar valores de dinero.
        public SqlMoney PrecioUnitario { get; set; }

        //es una estructura en .NET que permite almacenar información sobre fechas y horas.
        //En este caso, se usa para almacenar la fecha en la que se realizó el pedido.
        public DateTime FechaPedido { get; set; }
    }
}
