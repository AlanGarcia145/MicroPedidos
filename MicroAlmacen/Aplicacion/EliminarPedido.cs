using MediatR;
using MicroAlmacen.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace MicroAlmacen.Aplicacion
{
    public class EliminarPedido
    {
        public class ElimiPedido : IRequest<bool>
        {
            public int Id { get; set; }
        }

        public class Manejador : IRequestHandler<ElimiPedido, bool>
        {
            private readonly ContextoAlmacen _contexto;

            public Manejador(ContextoAlmacen contexto)
            {
                _contexto = contexto;
            }

            public async Task<bool> Handle(ElimiPedido request, CancellationToken cancellationToken)
            {
                var pedido = await _contexto.Pedido
                    .Where(x => x.Id == request.Id)
                    .FirstOrDefaultAsync();

                if (pedido == null)
                {
                    throw new Exception("No se encontró el pedido a eliminar");
                }

                _contexto.Pedido.Remove(pedido);
                await _contexto.SaveChangesAsync();

                return true;
            }
        }
    }
}
