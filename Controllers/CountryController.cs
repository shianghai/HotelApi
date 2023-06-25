using HotelApi.Data;
using HotelApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryController> _logger;
        public CountryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCountries()
        {
            try
            {
                var countries = await _unitOfWork.CountryRepository.GetAll();
                return Ok(countries);
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, $"something went wrong in {nameof(GetAllCountries)}");
                return StatusCode(500, "An error occured. Please try again later");
            }
        }

        [HttpGet("by/id/{id}:int")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var country = await _unitOfWork.CountryRepository.Get(c => c.Id == id);
                return Ok(country);
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, $"something went wrong in {nameof(GetById)}");
                return StatusCode(500, "An error occured. Please try again later");
            }
        }
    }
}
