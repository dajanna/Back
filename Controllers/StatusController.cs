using AutoMapper;
using FlightsReservation.Dto;
using FlightsReservation.Interface;
using FlightsReservation.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlightsReservation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : Controller
    {
        private readonly IStatusRepository _statusRepository;
        private readonly IMapper _mapper;

        public StatusController(IStatusRepository statusRepository, IMapper mapper)
        {
            _statusRepository = statusRepository;
            _mapper = mapper;

        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Status>))]
        public IActionResult GetListOfStatus()
        {
            var status = _mapper.Map<List<StatusDto>>(_statusRepository.GetListOfStatus());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(status);
        }
        [HttpGet("{statusId}")]
        [ProducesResponseType(200, Type = typeof(Status))]
        [ProducesResponseType(400)]

        public IActionResult GetStatus(int statusId)

        {
            if (!_statusRepository.StatusExists(statusId))
                return NotFound();
            var status = _mapper.Map<StatusDto>(_statusRepository.GetStatus(statusId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(status);


        }
        
        [HttpPost("CreateStatus")]
        public IActionResult CreateStatus([FromBody] StatusDto statusDto)
        {
            if (statusDto == null)
            {
                return BadRequest("Invalid status data.");
            }

          
            _statusRepository.InsertStatus(statusDto.Name);

            return Ok("Status created successfully.");
        }
        [HttpPut("UpdateStatus/{statusId}")]
        public IActionResult UpdateStatus(int statusId, [FromBody] StatusDto statusDto)
        {
            if (statusDto == null || statusDto.StatusID != statusId)
            {
                return BadRequest("Invalid status data.");
            }

            
            _statusRepository.UpdateStatus(statusId, statusDto.Name);

            return Ok("Status updated successfully.");
        }


    }
}
