using Application.Abstracts.Repositories.Cities;
using Application.Features.Cities.Dtos;
using AutoMapper;
using MediatR;

namespace Application.Features.Cities.Queries
{
    public class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQueryRequest, List<CityDto>>
    {
        readonly ICityReadRepository _readRepository;
        readonly IMapper _mapper;

        public GetAllCitiesQueryHandler(ICityReadRepository readRepository, IMapper mapper)
        {
            _readRepository = readRepository;
            _mapper = mapper;
        }

        public async Task<List<CityDto>> Handle(GetAllCitiesQueryRequest request, CancellationToken cancellationToken)
        {
            var cities = await _readRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);
            return _mapper.Map<List<CityDto>>(cities);
        }   
    }
}
