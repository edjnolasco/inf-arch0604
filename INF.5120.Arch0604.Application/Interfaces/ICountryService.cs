using INF._5120.Arch0604.Application.Common;
using INF._5120.Arch0604.Application.DTOs.CountryDTOs;

namespace INF._5120.Arch0604.Application.Interfaces
{
    public interface ICountryService
    {
        Task<ServiceResult<IEnumerable<CountryResponseDto>>> GetAllAsync();
        Task<ServiceResult<CountryResponseDto>> GetByIdAsync(int id);
        Task<ServiceResult<CountryResponseDto>> CreateAsync(CreateCountryRequestDto request);
        Task<ServiceResult<bool>> UpdateAsync(int id, UpdateCountryRequestDto request);
        Task<ServiceResult<bool>> DeleteAsync(int id);
    }
}