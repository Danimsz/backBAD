namespace BadServer.DataBase.Dto
    //Dto para la transferencia de datos entre cliente y servidor
{
    public class VerProductoCestaDto
    {
        public int ProductoID { get; set; }
        public string Modelo { get; set; }
        public int Cantidad { get; set; }
    }

    public class AgregarProductoDto
    {
        public int ProductoID { get; set; }
        public int Cantidad { get; set; }

    }

    public class QuitarProductoDto
    {
        public int ProductoID { get; set; }
    }
}
