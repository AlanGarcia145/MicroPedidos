using AutoMapper;
using MediatR;
using MicroAlmacen.Modelo;
using MicroAlmacen.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace MicroAlmacen.Aplicacion
{
    public class ObtenerPedidos
    {
        public class PedidosUsuario : IRequest<ICollection<PedidosDto>>
        {
            public string Email { get; set; }
        }

        public class Manejador : IRequestHandler<PedidosUsuario, ICollection<PedidosDto>>
        {
            private readonly ContextoAlmacen _contexto;
            private readonly IMapper _mapper;

            public Manejador(ContextoAlmacen contexto, IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;
            }

            public async Task<ICollection<PedidosDto>> Handle(PedidosUsuario request, CancellationToken cancellationToken)
            {
                var usuario = await _contexto.User
                    .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

                if (usuario == null)
                {
                    throw new Exception("No se encontró al usuario");
                }

                var pedidos = await _contexto.Pedido
                    .Where(p => p.Id_Usuario == usuario.Id)
                    .ToListAsync(cancellationToken);

                if (pedidos == null || !pedidos.Any())
                {
                    return new List<PedidosDto>();
                }

                var pedidosDto = _mapper.Map<ICollection<PedidosDto>>(pedidos);

                foreach (var dto in pedidosDto)
                {
                    if (!string.IsNullOrEmpty(dto.Fecha_Entrega))
                    {
                        var fecha = DateTime.Parse(dto.Fecha_Entrega);
                        dto.Fecha_Entrega = fecha.ToString("dddd dd MMMM yyyy", new System.Globalization.CultureInfo("es-ES"));
                    }
                }

                return pedidosDto;
            }
        }
    }
}
