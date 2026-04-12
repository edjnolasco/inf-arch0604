using INF._5120.Arch0604.Application.Common;
using INF._5120.Arch0604.Application.DTOs.CountryDTOs;
using INF._5120.Arch0604.Application.Interfaces;
using INF._5120.Arch0604.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace INF._5120.Arch0604.Application.Services
{
    public class CountryService(
        ICountryRepository countryRepository,
        ILogger<CountryService> logger) : ICountryService
    {
        public async Task<ServiceResult<IEnumerable<CountryResponseDto>>> GetAllAsync()
        {
            logger.LogInformation("Getting all countries.");

            var countries = await countryRepository.GetAllAsync();
            var response = countries.Select(MapToResponseDto);

            logger.LogInformation("Retrieved {Count} countries.", response.Count());

            return ServiceResult<IEnumerable<CountryResponseDto>>.Ok(response);
        }

        public async Task<ServiceResult<CountryResponseDto>> GetByIdAsync(int id)
        {
            logger.LogInformation("Getting country by id {CountryId}.", id);

            var country = await countryRepository.GetByIdAsync(id);

            if (country is null)
            {
                logger.LogWarning("Country with id {CountryId} was not found.", id);

                return ServiceResult<CountryResponseDto>.NotFound(
                    $"No se encontró el país con Id = {id}.");
            }

            logger.LogInformation("Country with id {CountryId} was found.", id);

            return ServiceResult<CountryResponseDto>.Ok(MapToResponseDto(country));
        }

        public async Task<ServiceResult<CountryResponseDto>> CreateAsync(CreateCountryRequestDto request)
        {
            var description = request.Description.Trim();
            var isoA2 = request.IsoA2.Trim().ToUpperInvariant();
            var isoA3 = request.IsoA3.Trim().ToUpperInvariant();

            logger.LogInformation(
                "Creating country with Description={Description}, IsoNum={IsoNum}, IsoA2={IsoA2}, IsoA3={IsoA3}.",
                description,
                request.IsoNum,
                isoA2,
                isoA3);

            var duplicated = await countryRepository.ExistsDuplicateAsync(
                description,
                request.IsoNum,
                isoA2,
                isoA3);

            if (duplicated)
            {
                logger.LogWarning(
                    "Duplicate country detected on create. Description={Description}, IsoNum={IsoNum}, IsoA2={IsoA2}, IsoA3={IsoA3}.",
                    description,
                    request.IsoNum,
                    isoA2,
                    isoA3);

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

            logger.LogInformation(
                "Country created successfully with id {CountryId}.",
                country.Id);

            return ServiceResult<CountryResponseDto>.Ok(
                MapToResponseDto(country),
                "País creado correctamente.");
        }

        public async Task<ServiceResult<bool>> UpdateAsync(int id, UpdateCountryRequestDto request)
        {
            logger.LogInformation("Updating country with route id {CountryId}.", id);

            if (id != request.Id)
            {
                logger.LogWarning(
                    "Route id {RouteId} does not match body id {BodyId}.",
                    id,
                    request.Id);

                return ServiceResult<bool>.Validation(
                    "El Id de la ruta no coincide con el Id enviado.");
            }

            var existingCountry = await countryRepository.GetByIdAsync(id);

            if (existingCountry is null)
            {
                logger.LogWarning("Country with id {CountryId} was not found for update.", id);

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
                logger.LogWarning(
                    "Duplicate country detected on update for id {CountryId}. Description={Description}, IsoNum={IsoNum}, IsoA2={IsoA2}, IsoA3={IsoA3}.",
                    id,
                    description,
                    request.IsoNum,
                    isoA2,
                    isoA3);

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

            logger.LogInformation("Country with id {CountryId} updated successfully.", id);

            return ServiceResult<bool>.Ok(true, "País actualizado correctamente.");
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            logger.LogInformation("Deleting country with id {CountryId}.", id);

            var existingCountry = await countryRepository.GetByIdAsync(id);

            if (existingCountry is null)
            {
                logger.LogWarning("Country with id {CountryId} was not found for deletion.", id);

                return ServiceResult<bool>.NotFound(
                    $"No se encontró el país con Id = {id}.");
            }

            countryRepository.Delete(existingCountry);
            await countryRepository.SaveChangesAsync();

            logger.LogInformation("Country with id {CountryId} deleted successfully.", id);

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