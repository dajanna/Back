using Microsoft.AspNetCore.SignalR;

namespace FlightsReservation.Hubs
{
    public class ReservationHub : Hub
    {

        public async Task SendReservationUpdate(int flightId, int availableSeats)
        {
            // Šalje svim klijentima (posetiocima i agentima) update o letu
            await Clients.All.SendAsync("ReceiveReservationUpdate", new
            {
                FlightId = flightId,
                AvailableSeats = availableSeats
            });
        }
    }
    
}
