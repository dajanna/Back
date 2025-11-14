using FlightsReservation.Data;
using FlightsReservation.Interface;
using FlightsReservation.Models;

namespace FlightsReservation.Repository
{
    public class FlightRepository : IFlightRepository
    {
        private readonly AppDbContext _context;


        private readonly string _connectionString;

        public FlightRepository(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public ICollection<Flight> GetListOfFlight()
        {
            return _context.Flights.ToList();
        }

        public Flight GetFlight(int id)
        {
            return _context.Flights.Where(a => a.Id == id).FirstOrDefault();
        }



        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }



        public void Insert(Flight flight)
        {
            _context.Flights.Add(flight);
        }

        public void Update(Flight flight)
        {
            _context.Flights.Update(flight);
        }

        public bool FlightExists(int id)
        {
            return _context.Flights.Any(c => c.Id == id);
        }
    }
}
