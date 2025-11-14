using AutoMapper;
using FlightsReservation.Dto;
using FlightsReservation.Models;

namespace FlightsReservation.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {


            CreateMap<Status, StatusDto>().ReverseMap();
            CreateMap<Reservation, ReservationDto>().ReverseMap();
            CreateMap<Flight, FlightDto>().ReverseMap();

        }
    }
}
