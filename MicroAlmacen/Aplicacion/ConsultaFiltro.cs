using AutoMapper;
using MediatR;
using MicroAlmacen.Modelo;
using MicroAlmacen.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace MicroAlmacen.Aplicacion
{
    public class ConsultaFiltro
    {
        public class AlmacenUnico : IRequest<AlmacenDto>
        {
            public int Codigo { get; set; }
        }
        public class Manejador : IRequestHandler<AlmacenUnico, AlmacenDto>
        {
            private readonly ContextoAlmacen _contexto;
            private readonly IMapper _mapper;

            public Manejador(ContextoAlmacen contexto, IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;
            }
            public async Task<AlmacenDto> Handle(AlmacenUnico request, CancellationToken cancellationToken)
            {
                var almacen = await _contexto.Almacen.
                    Where(x => x.Codigo == request.Codigo).FirstOrDefaultAsync();
                if (almacen == null)
                {
                    throw new Exception("No se encontro el almacen solicitado");
                }
                var almacenDto = _mapper.Map<Almacen, AlmacenDto>(almacen);
                return almacenDto;
            }
        }
    }
}
