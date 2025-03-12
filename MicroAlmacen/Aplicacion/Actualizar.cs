using FluentValidation;
using MediatR;
using MicroAlmacen.Modelo;
using MicroAlmacen.Persistencia;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace MicroAlmacen.Aplicacion
{
    public class Actualizar
    {
        public class Ejecuta : IRequest<Unit>
        {
            public int? Codigo { get; set; }
            public string NombreAlmacen { get; set; }
            public string Capacidad { get; set; }
            public string Ubicacion { get; set; }
            public string TipoAlmacen { get; set; }
            public int Producto { get; set; }
            public int Encargado { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Codigo).NotNull().WithMessage("El Código del almacén es obligatorio");
                RuleFor(x => x.NombreAlmacen).NotEmpty();
                RuleFor(x => x.Capacidad).NotEmpty();
                RuleFor(x => x.Ubicacion).NotEmpty();
                RuleFor(x => x.TipoAlmacen).NotEmpty();
                RuleFor(x => x.Producto).NotEmpty();
                RuleFor(x => x.Encargado).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, Unit>
        {
            private readonly ContextoAlmacen _contexto;
            public Manejador(ContextoAlmacen contexto)
            {
                _contexto = contexto;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var almacen = await _contexto.Almacen.FirstOrDefaultAsync(a => a.Codigo == request.Codigo, cancellationToken);

                if (almacen == null)
                {
                    throw new KeyNotFoundException($"No se encontró el almacén con el código {request.Codigo}");
                }

                almacen.NombreAlmacen = request.NombreAlmacen ?? almacen.NombreAlmacen;
                almacen.Capacidad = request.Capacidad ?? almacen.Capacidad;
                almacen.Ubicacion = request.Ubicacion ?? almacen.Ubicacion;
                almacen.TipoAlmacen = request.TipoAlmacen ?? almacen.TipoAlmacen;
                almacen.Producto = request.Producto != 0 ? request.Producto : almacen.Producto;
                almacen.Encargado = request.Encargado != 0 ? request.Encargado : almacen.Encargado;

                _contexto.Almacen.Update(almacen);
                var valor = await _contexto.SaveChangesAsync(cancellationToken);

                if (valor > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo actualizar el almacén");
            }
        }
    }
}
