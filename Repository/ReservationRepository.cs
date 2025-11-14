using FlightsReservation.Data;
using FlightsReservation.Interface;
using FlightsReservation.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FlightsReservation.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly AppDbContext _context;


        private readonly string _connectionString;

        public ReservationRepository(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public ICollection<Reservation> GetListOfReservation()
        {
            return _context.Reservations.ToList();
        }

        public Reservation GetReservation(int id)
        {
            return _context.Reservations.Where(a => a.Id == id).FirstOrDefault();
        }



        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }



        public void Insert(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
        }

        public void Update(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
        }

        public bool ReservationExists(int id)
        {
            return _context.Flights.Any(c => c.Id == id);
        }
        public Flight? GetFlightById(int flightId)
        {
            return _context.Flights.FirstOrDefault(f => f.Id == flightId);
        }
        public bool UserExists(string userId)
        {
            return _context.Users.Any(u => u.Id == userId);
        }


    }
}
