using MediatR;
using MicroAlmacen.Aplicacion;
using MicroAlmacen.Modelo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroAlmacen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PedidosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> CrearPedido([FromBody] CrearPedido.Crear data)
        {
            return await _mediator.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> ActualizarPedido(int id, [FromBody] ActualizarPedido.ActuPedido data)
        {
            return await _mediator.Send(data);
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<ICollection<PedidosDto>>> GetPedidosUsuario(string email)
        {
            if (string.IsNullOrEmpty(email) || !IsValidEmail(email))
            {
                return BadRequest("El email proporcionado no es válido.");
            }

            try
            {
                var pedidos = await _mediator.Send(new ObtenerPedidos.PedidosUsuario
                {
                    Email = email
                });

                if (pedidos == null || !pedidos.Any())
                {
                    return NotFound("No se encontraron pedidos para este usuario.");
                }

                return Ok(pedidos); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> EleminarPedido(int id)
        {
            return await _mediator.Send(new EliminarPedido.ElimiPedido
            {
                Id = id
            });
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
