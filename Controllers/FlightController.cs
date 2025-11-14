using AutoMapper;
using FlightsReservation.Data;
using FlightsReservation.Dto;
using FlightsReservation.Interface;
using FlightsReservation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightsReservation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightController : Controller
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;


        public FlightController(IFlightRepository flightRepository, IMapper mapper, AppDbContext context)
        {
            _flightRepository = flightRepository;
            _mapper = mapper;
            _context = context;

        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FlightDto>))]
       
        public IActionResult GetListOfFlights()
        {
            var flights = _mapper.Map<List<FlightDto>>(_flightRepository.GetListOfFlight());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(flights);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(FlightDto))]
        [ProducesResponseType(404)]
        public IActionResult GetFlight(int id)
        {
            if (!_flightRepository.FlightExists(id))
                return NotFound();

            var flight = _mapper.Map<FlightDto>(_flightRepository.GetFlight(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(flight);
        }
        private readonly string[] allowedCities = { "Beograd", "Niš", "Kraljevo", "Priština" };

        [HttpPost("CreateFlight")]
        [Authorize(Roles = "Agent")]
        public IActionResult CreateFlight([FromBody] FlightDto flightDto)
        {
            if (flightDto == null)
                return BadRequest("Invalid flight data");

            
            if (!allowedCities.Contains(flightDto.DepartureCity))
                return BadRequest("Invalid departure city");
            if (!allowedCities.Contains(flightDto.ArrivalCity))
                return BadRequest("Invalid arrival city");
            if (flightDto.DepartureCity == flightDto.ArrivalCity)
                return BadRequest("Departure city and Arrival city cannot be the same");

            var flight = _mapper.Map<Flight>(flightDto);
            _flightRepository.Insert(flight);
            if (!_flightRepository.Save())
                return StatusCode(500, "Error saving flight");

            return Ok(new { message = "Flight created successfully" });

        }

        [HttpPut("UpdateFlight/{id}")]
        public IActionResult UpdateFlight(int id, [FromBody] FlightDto flightDto)
        {
            if (flightDto == null || flightDto.Id != id)
                return BadRequest("Invalid flight data");

            var existing = _flightRepository.GetFlight(id);
            if (existing == null)
                return NotFound("Flight not found");

          
            if (!allowedCities.Contains(flightDto.DepartureCity))
                return BadRequest("Invalid departure city");
            if (!allowedCities.Contains(flightDto.ArrivalCity))
                return BadRequest("Invalid arrival city");
            if (flightDto.DepartureCity == flightDto.ArrivalCity)
                return BadRequest("Departure city and Arrival city cannot be the same");

            _mapper.Map(flightDto, existing);
            _flightRepository.Update(existing);
            if (!_flightRepository.Save())
                return StatusCode(500, "Error updating flight");

            return Ok("Flight updated successfully");
        }
       
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/status")]
        public IActionResult UpdateStatus(int id, [FromBody] string status)
        {
            var flight = _context.Flights.Find(id);
            if (flight == null) return NotFound();

            flight.Status = status;
            _context.SaveChanges();
            return Ok();
        }
    }
}
