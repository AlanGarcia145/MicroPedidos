using System.ComponentModel.DataAnnotations;

namespace MicroAlmacen.Modelo
{
    public class Pedido
    {
        [Key]
        public int? Id { get; set; }
        public int Cantidad { get; set; }
        public string Direccion { get; set; }
        public DateTime Fecha_Entrega { get; set; }
        public double Total { get; set; }
        public string Latitude { get; set; }
        public string Longitud { get; set; }
        public int Id_Usuario { get; set; }
    }
}
