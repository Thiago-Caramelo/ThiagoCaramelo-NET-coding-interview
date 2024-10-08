using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SecureFlight.Core.Entities;
using SecureFlight.Core.Interfaces;

namespace SecureFlight.Core.Services;

public class FlightService(IRepository<Flight> flightRepository, IRepository<Passenger> passengerRepository)
{
    public async Task<OperationResult<IReadOnlyList<Flight>>> GetAllAsync()
    {
        return OperationResult<IReadOnlyList<Flight>>.Success(await flightRepository.GetAllAsync());
    }

    public async Task<OperationResult<IReadOnlyList<Flight>>> FilterAsync(Expression<Func<Flight, bool>> predicate)
    {
        return OperationResult<IReadOnlyList<Flight>>.Success(await flightRepository.FilterAsync(predicate));
    }

    public async Task<OperationResult<Flight>> FindAsync(params object[] keyValues)
    {
        var entity = await flightRepository.GetByIdAsync(keyValues);
        return entity is null ?
            OperationResult<Flight>.NotFound($"Entity with key values {string.Join(", ", keyValues)} was not found") :
            OperationResult<Flight>.Success(entity);
    }

    public async Task<OperationResult<Flight>> AddPassengerToFlight(long flightId, string passengerId)
    {
        var flight = await flightRepository.GetByIdAsync(flightId);

        if (flight is null)
        {
            return OperationResult<Flight>.NotFound($"Entity with key values {string.Join(", ", flightId)} was not found");
        }

        var passenger = await passengerRepository.GetByIdAsync(passengerId);

        if (passenger is null)
        {
            return OperationResult<Flight>.NotFound($"Entity with key values {string.Join(", ", passengerId)} was not found");
        }

        flight.Passengers.Add(passenger);

        await flightRepository.Update(flight);

        return OperationResult<Flight>.Success(flight);
    }
}