using HotelApi.DTOS.WriteDtos;

namespace HotelApi.Interfaces
{
    public interface IAuthManager
    {
        Task<bool> AuthenticateUserAsync(LoginWriteDto loginInfo);

        Task<string> GenerateTokenAsync();
    }
}
