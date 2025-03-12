using MediatR;
using MicroAlmacen.Aplicacion;
using MicroAlmacen.Persistencia;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MicroAlmacen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlmacenController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ContextoAlmacen _contextoAlmacen;

        public AlmacenController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            return await _mediator.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> ActualizarAlmacen(int id, [FromBody] Actualizar.Ejecuta data)
        {
            data.Codigo = id; // Asignar el ID desde la URL al objeto data

            return await _mediator.Send(data);
        }

        [HttpGet]
        public async Task<ActionResult<List<AlmacenDto>>> GetAlmacenes()
        {
            return await _mediator.Send(new Consulta.Ejecuta());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AlmacenDto>> GetAlmacenUnico(int id)
        {
            return await _mediator.Send(new ConsultaFiltro.AlmacenUnico
            {
                Codigo = id
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Eleminar(int id)
        {
            return await _mediator.Send(new Eliminar.EliminarAlmacen
            {
                Codigo = id
            });
        }
    }
}
