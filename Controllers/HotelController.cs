using AutoMapper;
using HotelApi.DTOS.ReadDtos;
using HotelApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HotelController> _logger;
        private readonly IMapper _mapper;

        public HotelController(IUnitOfWork unitOfWork, ILogger<HotelController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            
                var hotels = await _unitOfWork.HotelRepository.GetAll();
                if(hotels.Count > 0)
                {
                    var mappedHotels = _mapper.Map<List<HotelReadDto>>(hotels);
                    return Ok(mappedHotels);
                }
                return StatusCode(200, "No Hotels Found");
           
        }

        [Authorize(Roles="admin")]
        [HttpGet("by/id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var hotel = await _unitOfWork.HotelRepository.Get(h => h.Id == id);
                if (hotel is not null) {
                    var mappedHotel = _mapper.Map<HotelReadDto>(hotel);
                    return Ok(mappedHotel);
                 };
                return StatusCode(200, "No item found for the specified Id");
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, $"Something went wrong in {nameof(HotelController.GetById)}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
