using INF._5120.Arch0604.Application.Common;
using INF._5120.Arch0604.Application.DTOs.CountryDTOs;
using INF._5120.Arch0604.Application.Interfaces;
using INF._5120.Arch0604.Domain.Entities;

namespace INF._5120.Arch0604.Application.Services
{
    public class CountryService(ICountryRepository countryRepository) : ICountryService
    {
        public async Task<ServiceResult<IEnumerable<CountryResponseDto>>> GetAllAsync()
        {
            var countries = await countryRepository.GetAllAsync();
            var response = countries.Select(MapToResponseDto);

            return ServiceResult<IEnumerable<CountryResponseDto>>.Ok(response);
        }

        public async Task<ServiceResult<CountryResponseDto>> GetByIdAsync(int id)
        {
            var country = await countryRepository.GetByIdAsync(id);

            if (country is null)
            {
                return ServiceResult<CountryResponseDto>.NotFound(
                    $"No se encontró el país con Id = {id}.");
            }

            return ServiceResult<CountryResponseDto>.Ok(MapToResponseDto(country));
        }

        public async Task<ServiceResult<CountryResponseDto>> CreateAsync(CreateCountryRequestDto request)
        {
            var description = request.Description.Trim();
            var isoA2 = request.IsoA2.Trim().ToUpperInvariant();
            var isoA3 = request.IsoA3.Trim().ToUpperInvariant();

            var duplicated = await countryRepository.ExistsDuplicateAsync(
                description,
                request.IsoNum,
                isoA2,
                isoA3);

            if (duplicated)
            {
                return ServiceResult<CountryResponseDto>.Conflict(
                    "Ya existe un país con la misma descripción, ISO numérico, ISO A2 o ISO A3.");
            }

            var country = new Country
            {
                Description = description,
                IsoNum = request.IsoNum,
                IsoA2 = isoA2,
                IsoA3 = isoA3,
                IsEnable = request.IsEnable,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = null
            };

            await countryRepository.AddAsync(country);
            await countryRepository.SaveChangesAsync();

            return ServiceResult<CountryResponseDto>.Ok(
                MapToResponseDto(country),
                "País creado correctamente.");
        }

        public async Task<ServiceResult<bool>> UpdateAsync(int id, UpdateCountryRequestDto request)
        {
            if (id != request.Id)
            {
                return ServiceResult<bool>.Validation(
                    "El Id de la ruta no coincide con el Id enviado.");
            }

            var existingCountry = await countryRepository.GetByIdAsync(id);

            if (existingCountry is null)
            {
                return ServiceResult<bool>.NotFound(
                    $"No se encontró el país con Id = {id}.");
            }

            var description = request.Description.Trim();
            var isoA2 = request.IsoA2.Trim().ToUpperInvariant();
            var isoA3 = request.IsoA3.Trim().ToUpperInvariant();

            var duplicated = await countryRepository.ExistsDuplicateAsync(
                description,
                request.IsoNum,
                isoA2,
                isoA3,
                id);

            if (duplicated)
            {
                return ServiceResult<bool>.Conflict(
                    "Ya existe otro país con la misma descripción, ISO numérico, ISO A2 o ISO A3.");
            }

            existingCountry.Description = description;
            existingCountry.IsoNum = request.IsoNum;
            existingCountry.IsoA2 = isoA2;
            existingCountry.IsoA3 = isoA3;
            existingCountry.IsEnable = request.IsEnable;
            existingCountry.UpdatedDate = DateTime.UtcNow;

            await countryRepository.SaveChangesAsync();

            return ServiceResult<bool>.Ok(true, "País actualizado correctamente.");
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            var existingCountry = await countryRepository.GetByIdAsync(id);

            if (existingCountry is null)
            {
                return ServiceResult<bool>.NotFound(
                    $"No se encontró el país con Id = {id}.");
            }

            countryRepository.Delete(existingCountry);
            await countryRepository.SaveChangesAsync();

            return ServiceResult<bool>.Ok(true, "País eliminado correctamente.");
        }

        private static CountryResponseDto MapToResponseDto(Country country)
        {
            return new CountryResponseDto
            {
                Id = country.Id,
                Description = country.Description,
                IsoNum = country.IsoNum,
                IsoA2 = country.IsoA2,
                IsoA3 = country.IsoA3,
                IsEnable = country.IsEnable,
                CreatedDate = country.CreatedDate,
                UpdatedDate = country.UpdatedDate
            };
        }
    }
}