using FluentValidation;
using MediatR;
using MicroAlmacen.Modelo;
using MicroAlmacen.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace MicroAlmacen.Aplicacion
{
    public class CrearPedido
    {
        public class Crear : IRequest<Unit>
        {
            public int Cantidad { get; set; }
            public string Direccion { get; set; }
            public DateTime FechaEntrega { get; set; }
            public double Total { get; set; }
            public string Latitude { get; set; }
            public string Longitude { get; set; }
            public string Usuario { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Crear>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Cantidad).NotEmpty();
                RuleFor(x => x.Direccion).NotEmpty();
                RuleFor(x => x.FechaEntrega).NotEmpty();
                RuleFor(x => x.Total).NotEmpty();
                RuleFor(x => x.Latitude).NotEmpty();
                RuleFor(x => x.Longitude).NotEmpty();
                RuleFor(x => x.Usuario).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Crear, Unit>
        {
            private readonly ContextoAlmacen _contexto;
            public Manejador(ContextoAlmacen contexto)
            {
                _contexto = contexto;
            }

            public async Task<Unit> Handle(Crear request, CancellationToken cancellationToken)
            {
                var usuario = await _contexto.User.Where(u => u.Email == request.Usuario).FirstOrDefaultAsync();

                if (usuario == null)
                {
                    throw new Exception("No se encontro al usuario");
                }

                var pedido = new Pedido
                {
                    Cantidad = request.Cantidad,
                    Direccion = request.Direccion,
                    Fecha_Entrega = request.FechaEntrega,
                    Total = request.Total,
                    Latitude = request.Latitude,
                    Longitud = request.Longitude,
                    Id_Usuario = usuario.Id,
                };

                _contexto.Pedido.Add(pedido);
                var valor = await _contexto.SaveChangesAsync();
                if (valor > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo guardar el pedido");
            }
        }
    }
}
