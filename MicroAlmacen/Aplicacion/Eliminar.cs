using MediatR;
using MicroAlmacen.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace MicroAlmacen.Aplicacion
{
    public class Eliminar
    {
        public class EliminarAlmacen : IRequest<bool>
        {
            public int Codigo { get; set; }
        }

        public class Manejador : IRequestHandler<EliminarAlmacen, bool>
        {
            private readonly ContextoAlmacen _contexto;

            public Manejador(ContextoAlmacen contexto)
            {
                _contexto = contexto;
            }

            public async Task<bool> Handle(EliminarAlmacen request, CancellationToken cancellationToken)
            {
                var almacen = await _contexto.Almacen
                    .Where(x => x.Codigo == request.Codigo)
                    .FirstOrDefaultAsync();

                if (almacen == null)
                {
                    throw new Exception("No se encontró el almacen a eliminar");
                }

                _contexto.Almacen.Remove(almacen);
                await _contexto.SaveChangesAsync();

                return true;
            }
        }
    }
}
