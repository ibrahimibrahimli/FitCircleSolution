using Application.Features.Cities.Commands.Create;
using Application.Features.Cities.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FitCircleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        public readonly IMediator _mediatr;

        public CityController(IMediator mediatr)
        {
            _mediatr = mediatr;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCityDto dto)
        {
            var commandRequest = new CreateCityCommandRequest(dto);
            var city = await _mediatr.Send(commandRequest);
            return CreatedAtAction(nameof(Create), city);
        }
    }
}
