using FluentValidation;
using MediatR;
using MicroAlmacen.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace MicroAlmacen.Aplicacion
{
    public class ActualizarPedido
    {
        public class ActuPedido : IRequest<Unit>
        {
            public int Id { get; set; }
            public int Cantidad { get; set; }
            public string Direccion { get; set; }
            public DateTime FechaEntrega { get; set; }
            public double Total { get; set; }
            public string Latitude { get; set; }
            public string Longitude { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<ActuPedido>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Id).NotNull().WithMessage("El id es obligatorio");
                RuleFor(x => x.Cantidad).NotEmpty();
                RuleFor(x => x.Direccion).NotEmpty();
                RuleFor(x => x.FechaEntrega).NotEmpty();
                RuleFor(x => x.Total).NotEmpty();
                RuleFor(x => x.Latitude).NotEmpty();
                RuleFor(x => x.Longitude).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<ActuPedido, Unit>
        {
            private readonly ContextoAlmacen _contexto;
            public Manejador(ContextoAlmacen contexto)
            {
                _contexto = contexto;
            }

            public async Task<Unit> Handle(ActuPedido request, CancellationToken cancellationToken)
            {
                var pedido = await _contexto.Pedido.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

                if (pedido == null)
                {
                    throw new KeyNotFoundException($"No se encontró el pedido con el id {request.Id}");
                }

                pedido.Cantidad = request.Cantidad;
                pedido.Direccion = request.Direccion ?? pedido.Direccion;
                pedido.Fecha_Entrega = request.FechaEntrega;
                pedido.Total = request.Total;
                pedido.Latitude = request.Latitude;
                pedido.Longitud = request.Longitude;

                _contexto.Pedido.Update(pedido);
                var valor = await _contexto.SaveChangesAsync(cancellationToken);

                if (valor > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo actualizar el pedido");
            }
        }
    }
}
