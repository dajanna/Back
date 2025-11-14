using FlightsReservation.Models;

namespace FlightsReservation.Interface
{
    public interface IFlightRepository
    {
        ICollection<Flight> GetListOfFlight();
        Flight GetFlight(int id);
        bool FlightExists(int id);
        bool Save();
        void Insert(Flight flight);
        void Update(Flight flight);
    }
}
