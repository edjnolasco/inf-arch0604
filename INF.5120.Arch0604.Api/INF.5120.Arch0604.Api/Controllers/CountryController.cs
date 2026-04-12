using INF._5120.Arch0604.Application.DTOs.CountryDTOs;
using INF._5120.Arch0604.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace INF._5120.Arch0604.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController(ICountryService countryService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryResponseDto>>> GetCountries()
        {
            var result = await countryService.GetAllAsync();
            return Ok(result.Data);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CountryResponseDto>> GetCountryById(int id)
        {
            var result = await countryService.GetByIdAsync(id);

            if (!result.Success)
            {
                return NotFound(new { result.Message });
            }

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<ActionResult<CountryResponseDto>> CreateCountry([FromBody] CreateCountryRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var result = await countryService.CreateAsync(request);

            if (!result.Success)
            {
                return Conflict(new { result.Message });
            }

            return CreatedAtAction(
                nameof(GetCountryById),
                new { id = result.Data!.Id },
                result.Data);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var result = await countryService.UpdateAsync(id, request);

            if (!result.Success)
            {
                if (result.Message.Contains("no coincide"))
                    return BadRequest(new { result.Message });

                if (result.Message.Contains("No se encontró"))
                    return NotFound(new { result.Message });

                return Conflict(new { result.Message });
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var result = await countryService.DeleteAsync(id);

            if (!result.Success)
            {
                return NotFound(new { result.Message });
            }

            return NoContent();
        }
    }
}