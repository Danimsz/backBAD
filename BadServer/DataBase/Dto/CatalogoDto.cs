namespace BadServer.DataBase.Dto
{
    public class CatalogoDto
    {
        public int ProductoID { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public ImagenDto Imagen { get; set; }
    }
}
