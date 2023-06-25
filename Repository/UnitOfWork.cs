using HotelApi.Data;
using HotelApi.Interfaces;

namespace HotelApi.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HotelDbContext _context;
        private IGenericRepository<Country> _countryRepo;
        private IGenericRepository<Hotel> _hotelRepo;
        public UnitOfWork(HotelDbContext context)
        {
            _context = context;
        }
        public IGenericRepository<Country> CountryRepository => _countryRepo ??= new GenericRepository<Country>(_context);

        public IGenericRepository<Hotel> HotelRepository => _hotelRepo ??= new GenericRepository<Hotel>(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
