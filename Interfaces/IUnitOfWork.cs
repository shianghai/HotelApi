using HotelApi.Data;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace HotelApi.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        public IGenericRepository<Country> CountryRepository { get; }

        public IGenericRepository<Hotel> HotelRepository { get; }

        public void SaveChanges();
    }
}
