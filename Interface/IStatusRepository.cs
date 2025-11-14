using FlightsReservation.Models;

namespace FlightsReservation.Interface
{
    public interface IStatusRepository
    {
        ICollection<Status> GetListOfStatus();
        Status GetStatus(int id);
        bool StatusExists(int id);
        bool Save();
        void InsertStatus(string name);
        void UpdateStatus(int statusId, string name);

    }
}
