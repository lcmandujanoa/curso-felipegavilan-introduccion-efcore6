using EFCorePeliculas.Entidades;
using EFCorePeliculas.Entidades.Funciones;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCorePeliculas.Controllers
{
    [ApiController]
    [Route("api/facturas")]
    public class FacturasController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public FacturasController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("{id:int}/detalle")]
        public async Task<ActionResult<IEnumerable<FacturaDetalle>>> GetDetalle(int id)
        {
            return await context.FacturaDetalles.Where(f => f.FacturaId == id)
                .OrderByDescending(f => f.Total)
                .ToListAsync();
        }


        [HttpPost("Concurrencia_Fila")]
        public async Task<ActionResult> ConcurrenciaFila()
        {
            var facturaId = 2;

            var factura = await context.Facturas.AsTracking().FirstOrDefaultAsync(f => f.Id == facturaId);
            factura.FechaCreacion = DateTime.Now;

            await context.Database.ExecuteSqlInterpolatedAsync(
                                                     @$"UPDATE Facturas SET FechaCreacion = GetDate()
                                                        WHERE Id = {facturaId}");

            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("Funciones_escalares")]
        public async Task<ActionResult> GetFuncionesEscalares()
        {
            var facturas = await context.Facturas.Select(f => new
            { 
                Id = f.Id,
                Total = context.FacturaDetalleSuma(f.Id),
                Promedio = Escalares.FacturaDetallePromedio(f.Id)
            })
                .OrderByDescending(f => context.FacturaDetalleSuma(f.Id))
                .ToListAsync();

            return Ok(facturas);
        }

        [HttpPost]
        public async Task<ActionResult> Post()
        {
            using var transaccion = await context.Database.BeginTransactionAsync();
            try
            {
                var factura = new Factura()
                {
                    FechaCreacion = DateTime.Now
                };

                context.Add(factura);
                await context.SaveChangesAsync();

                //throw new ApplicationException("esto es una prueba");

                var facturaDetalle = new List<FacturaDetalle>()
                {
                    new FacturaDetalle()
                    {
                        Producto = "Producto A",
                        Precio = 123,
                        FacturaId = factura.Id
                    },
                    new FacturaDetalle()
                    {
                        Producto = "Producto B",
                        Precio = 456,
                        FacturaId = factura.Id
                    },
                };

                context.AddRange(facturaDetalle);
                await context.SaveChangesAsync();
                await transaccion.CommitAsync();
                return Ok("todo bien");
            }
            catch (Exception ex)
            {
                //Manejar Excepción
                //return BadRequest("Hubo un error");
                return BadRequest(ex.Message);
            }

        }
    }
}
