using Org.BouncyCastle.Math;
using System.ComponentModel.DataAnnotations;

namespace MicroAlmacen.Modelo
{
    public class Almacen
    {
        [Key]
        public int? Codigo { get; set; } 
        public string NombreAlmacen { get; set; }
        public string Capacidad { get; set; }
        public string Ubicacion { get; set; }
        public string TipoAlmacen { get; set; } 
        public int Producto { get; set; }
        public int Encargado { get; set; }

    }
}
