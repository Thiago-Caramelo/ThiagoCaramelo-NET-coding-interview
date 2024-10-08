using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SecureFlight.Api.Models;
using SecureFlight.Api.Utils;
using SecureFlight.Core.Entities;
using SecureFlight.Core.Interfaces;
using SecureFlight.Core.Services;

namespace SecureFlight.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FlightsController(FlightService flightService, IMapper mapper)
    : SecureFlightBaseController(mapper)
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FlightDataTransferObject>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponseActionResult))]
    public async Task<IActionResult> Get()
    {
        var flights = await flightService.GetAllAsync();
        return MapResultToDataTransferObject<IReadOnlyList<Flight>, IReadOnlyList<FlightDataTransferObject>>(flights);
    }

    [HttpPost("{flightId:long}/passengers/{passengerId}")]
    //[ProducesResponseType(typeof(IEnumerable<PassengerDataTransferObject>), StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponseActionResult))]
    public async Task<IActionResult> AddPassengerToFlight(long flightId, string passengerId)
    {
        var response = await flightService.AddPassengerToFlight(flightId, passengerId);

        if (response.Succeeded)
        {
            return Created();
        }

        return BadRequest(response.ErrorResult.Code);
    }

}