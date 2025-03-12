using AutoMapper;
using MediatR;
using MicroAlmacen.Modelo;
using MicroAlmacen.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace MicroAlmacen.Aplicacion
{
    public class Consulta
    {
        public class Ejecuta : IRequest<List<AlmacenDto>>
        {
            public Ejecuta()
            {

            }
        }

        public class Manejador : IRequestHandler<Ejecuta, List<AlmacenDto>>
        {
            private readonly ContextoAlmacen _contexto;
            private readonly IMapper _mapper;

            public Manejador(ContextoAlmacen contexto, IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;
            }

            public async Task<List<AlmacenDto>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var almacenes = await _contexto.Almacen.ToListAsync();
                var almacenesDto = _mapper.Map<List<Almacen>, List<AlmacenDto>>(almacenes);
                return almacenesDto;
            }
        }
    }
}
