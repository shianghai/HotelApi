using HotelApi.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApi.DTOS.ReadDtos
{
    public class HotelReadDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}
