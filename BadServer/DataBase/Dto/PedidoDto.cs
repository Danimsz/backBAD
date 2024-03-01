namespace BadServer.DataBase.Dto
{
    public class PedidoDto
    {
        public int ClienteID { get; set; }
        public string MetodoPago { get; set; }
        public decimal Total { get; set; }
        public int Estado { get; set; }
        public decimal PrecioEuro { get; set; }
        public double PrecioEtherium { get; set; }
        public string? HashTransaccion { get; set; }
        public string? WalletCliente { get; set; }
        public DateTime FechaPedido { get; set; }
        public List<ProductoDto> Productos { get; set; }
    }

    public class ProductoDto
    {
        public int ProductoID { get; set; }
        public int Cantidad { get; set; }
        public double PrecioUnitario { get; set; }
    }
}