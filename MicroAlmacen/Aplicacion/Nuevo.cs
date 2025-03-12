using FluentValidation;
using MediatR;
using MicroAlmacen.Modelo;
using MicroAlmacen.Persistencia;

namespace MicroAlmacen.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest<Unit>
        {
            public int Codigo { get; set; }
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
                    RuleFor(x => x.Codigo).NotEmpty();
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
                    var almacen = new Almacen
                    {
                        Codigo = request.Codigo,
                        NombreAlmacen = request.NombreAlmacen,
                        Capacidad = request.Capacidad,
                        Ubicacion = request.Ubicacion,
                        TipoAlmacen = request.TipoAlmacen,
                        Producto = request.Producto,
                        Encargado = request.Encargado
                    };

                    _contexto.Almacen.Add(almacen);
                    var valor = await _contexto.SaveChangesAsync();
                    if (valor > 0)
                    {
                        return Unit.Value;
                    }
                    throw new Exception("No se pudo guardar el almacen");
                }
            }
        
    }

}