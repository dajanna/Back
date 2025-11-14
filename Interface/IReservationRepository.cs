using FlightsReservation.Models;

namespace FlightsReservation.Interface
{
    public interface IReservationRepository
    {
        ICollection<Reservation> GetListOfReservation();
        Reservation GetReservation(int id);
        bool ReservationExists(int id);
        bool Save();
        void Insert(Reservation reservation);
        void Update(Reservation reservation);
        Flight? GetFlightById(int flightId);
        bool UserExists(string userId);
    }
}
