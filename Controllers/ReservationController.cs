using AutoMapper;
using FlightsReservation.Data;
using FlightsReservation.Dto;
using FlightsReservation.Hubs;
using FlightsReservation.Interface;
using FlightsReservation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FlightsReservation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class ReservationController : Controller
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<ReservationHub> _hubContext;
        private readonly IFlightRepository _flightRepository;
        private readonly AppDbContext _context;
        public ReservationController(IReservationRepository reservationRepository, IMapper mapper, IHubContext<ReservationHub> hubContext, IFlightRepository flightRepository, AppDbContext context)
        {
            _reservationRepository = reservationRepository;
            _mapper = mapper;
            _hubContext = hubContext;
            _flightRepository = flightRepository;
            _context = context; 
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReservationDto>))]
        public IActionResult GetReservations()
        {
            var reservations = _mapper.Map<List<ReservationDto>>(_reservationRepository.GetListOfReservation());
            return Ok(reservations);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ReservationDto))]
        [ProducesResponseType(404)]
        public IActionResult GetReservation(int id)
        {
            var reservation = _reservationRepository.GetReservation(id);
            if (reservation == null)
                return NotFound("Reservation not found");

            return Ok(_mapper.Map<ReservationDto>(reservation));
        }
       
        [Authorize(Roles = "Posetilac")]
        [HttpPost("CreateReservation")]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationDto reservationDto)
        {
           
            var existingReservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.UserId == reservationDto.UserId && r.FlightId == reservationDto.FlightId);

            if (existingReservation != null)
            {
                return BadRequest("You have already reserved seats on this flight.");
            }

            
            var flight = await _context.Flights.FindAsync(reservationDto.FlightId);
            if (flight == null)
                return NotFound("Flight not found.");

            if (flight.AvailableSeats < reservationDto.SeatsReserved)
                return BadRequest("Not enough available seats.");

            
            var reservation = new Reservation
            {
                SeatsReserved = reservationDto.SeatsReserved,
                ReservationDate = DateTime.Now,
                StatusId = 1, 
                FlightId = reservationDto.FlightId,
                UserId = reservationDto.UserId
            };

            
            flight.AvailableSeats -= reservation.SeatsReserved;

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return Ok("Reservation created successfully.");
        }



        [HttpPut("UpdateReservation/{id}")]
        public IActionResult UpdateReservation(int id, [FromBody] ReservationDto reservationDto)
        {
            if (reservationDto == null || reservationDto.Id != id)
                return BadRequest("Invalid reservation data");

            var existing = _reservationRepository.GetReservation(id);
            if (existing == null)
                return NotFound("Reservation not found");

          
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated");

            
            if (_reservationRepository.GetFlightById(reservationDto.FlightId) == null)
                return BadRequest("Flight not found");

           
            _mapper.Map(reservationDto, existing);
            existing.UserId = userId; 
            _reservationRepository.Update(existing);

            if (!_reservationRepository.Save())
                return StatusCode(500, "Error updating reservation");

            return Ok("Reservation updated successfully");
        }

        [Authorize(Roles = "Agent")]
        [HttpPatch("ApproveReservation/{id}")]
        public async Task<IActionResult> ApproveReservation(int id)
        {
            
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return NotFound();

          
            reservation.StatusId = 2;
            await _context.SaveChangesAsync();

            
            await _hubContext.Clients.All.SendAsync("ReservationApproved", new
            {
                ReservationId = reservation.Id,
                FlightId = reservation.FlightId,
                StatusId = reservation.StatusId
            });

           
            return Ok(new { reservation.Id, reservation.StatusId });
        }
        [HttpGet("MyReservations")]
        [Authorize(Roles = "Posetilac")]
        public async Task<IActionResult> GetMyReservations()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var reservations = await _context.Reservations
                .Where(r => r.UserId == userId)
                .Select(r => new ReservationForUserDto
                {
                    Id = r.Id,
                    SeatsReserved = r.SeatsReserved,
                    ReservationDate = r.ReservationDate,
                    StatusName = r.Status.Name,
                    FlightId = r.FlightId,
                    Flight = new FlightDto
                    {
                        Id = r.Flight.Id,
                        DepartureCity = r.Flight.DepartureCity,
                        ArrivalCity = r.Flight.ArrivalCity,
                        FlightDate = r.Flight.FlightDate,
                        AvailableSeats = r.Flight.AvailableSeats,
                        Status = r.Flight.Status
                    }
                })
                .ToListAsync();

            return Ok(reservations);

        }
        [HttpGet("AllReservations")]
        public async Task<IActionResult> GetAllReservations()
        {
            var reservations = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Status)
                .Include(r => r.Flight)
                .Select(r => new ReservationForAgentDto
                {
                    Id = r.Id,
                    FlightId = r.FlightId,
                    UserName = r.User.FullName, // ili r.User.FullName zavisno od modela
                    SeatsReserved = r.SeatsReserved,
                    ReservationDate = r.ReservationDate,
                    StatusName = r.Status.Name
                })
                .ToListAsync();

            return Ok(reservations);
        }
    }
}


