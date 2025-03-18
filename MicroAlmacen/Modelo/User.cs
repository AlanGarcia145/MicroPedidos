using System.ComponentModel.DataAnnotations;

namespace MicroAlmacen.Modelo
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
    }
}
