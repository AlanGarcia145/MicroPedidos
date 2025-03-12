namespace MicroAlmacen.Aplicacion
{
    public class AlmacenDto
    {
        public int? Codigo { get; set; }
        public string NombreAlmacen { get; set; }
        public string Capacidad { get; set; }
        public string Ubicacion { get; set; }
        public string TipoAlmacen { get; set; }
        public int Producto { get; set; }
        public int Encargado { get; set; }
    }
}
