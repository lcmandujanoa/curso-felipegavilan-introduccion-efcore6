using EFCorePeliculas.Entidades;

namespace EFCorePeliculas.DTOs
{
    public class SalaDeCineCreacionDTO
    {
        public TipoSalaDeCine TipoSalaDeCine { get; set; }
        public decimal Precio { get; set; }
    }
}
