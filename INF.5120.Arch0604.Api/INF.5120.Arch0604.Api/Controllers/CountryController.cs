using INF._5120.Arch0604.Application.Common;
using INF._5120.Arch0604.Application.DTOs.CountryDTOs;
using INF._5120.Arch0604.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace INF._5120.Arch0604.Api.Controllers
{
    /// <summary>
    /// Controlador para la gestión de países.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CountryController(ICountryService countryService) : ControllerBase
    {
        /// <summary>
        /// Obtiene la lista de todos los países.
        /// </summary>
        /// <returns>Lista de países registrados.</returns>
        /// <response code="200">Lista obtenida correctamente.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CountryResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CountryResponseDto>>> GetCountries()
        {
            var result = await countryService.GetAllAsync();
            return Ok(result.Data);
        }

        /// <summary>
        /// Obtiene un país por su identificador.
        /// </summary>
        /// <param name="id">Identificador del país.</param>
        /// <returns>El país encontrado.</returns>
        /// <response code="200">País encontrado.</response>
        /// <response code="404">País no encontrado.</response>
        /// <response code="400">Solicitud inválida.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(CountryResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CountryResponseDto>> GetCountryById(int id)
        {
            var result = await countryService.GetByIdAsync(id);

            if (!result.Success)
            {
                return result.ErrorType switch
                {
                    ServiceErrorType.NotFound => NotFound(new { result.Message }),
                    ServiceErrorType.Validation => BadRequest(new { result.Message }),
                    ServiceErrorType.Conflict => Conflict(new { result.Message }),
                    _ => BadRequest(new { result.Message })
                };
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// Crea un nuevo país.
        /// </summary>
        /// <param name="request">Datos del país a crear.</param>
        /// <returns>El país creado.</returns>
        /// <response code="201">País creado correctamente.</response>
        /// <response code="400">Datos inválidos.</response>
        /// <response code="409">Conflicto por duplicidad de datos.</response>
        [HttpPost]
        [ProducesResponseType(typeof(CountryResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CountryResponseDto>> CreateCountry([FromBody] CreateCountryRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var result = await countryService.CreateAsync(request);

            if (!result.Success)
            {
                return result.ErrorType switch
                {
                    ServiceErrorType.Conflict => Conflict(new { result.Message }),
                    ServiceErrorType.Validation => BadRequest(new { result.Message }),
                    ServiceErrorType.NotFound => NotFound(new { result.Message }),
                    _ => BadRequest(new { result.Message })
                };
            }

            return CreatedAtAction(
                nameof(GetCountryById),
                new { id = result.Data.Id },
                result.Data);
        }

        /// <summary>
        /// Actualiza un país existente.
        /// </summary>
        /// <param name="id">Identificador del país.</param>
        /// <param name="request">Datos actualizados del país.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <response code="204">Actualización exitosa.</response>
        /// <response code="400">Solicitud inválida o inconsistente.</response>
        /// <response code="404">País no encontrado.</response>
        /// <response code="409">Conflicto por duplicidad de datos.</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var result = await countryService.UpdateAsync(id, request);

            if (!result.Success)
            {
                return result.ErrorType switch
                {
                    ServiceErrorType.Validation => BadRequest(new { result.Message }),
                    ServiceErrorType.NotFound => NotFound(new { result.Message }),
                    ServiceErrorType.Conflict => Conflict(new { result.Message }),
                    _ => BadRequest(new { result.Message })
                };
            }

            return NoContent();
        }

        /// <summary>
        /// Elimina un país por su identificador.
        /// </summary>
        /// <param name="id">Identificador del país.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <response code="204">Eliminación exitosa.</response>
        /// <response code="404">País no encontrado.</response>
        /// <response code="400">Solicitud inválida.</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var result = await countryService.DeleteAsync(id);

            if (!result.Success)
            {
                return result.ErrorType switch
                {
                    ServiceErrorType.NotFound => NotFound(new { result.Message }),
                    ServiceErrorType.Validation => BadRequest(new { result.Message }),
                    ServiceErrorType.Conflict => Conflict(new { result.Message }),
                    _ => BadRequest(new { result.Message })
                };
            }

            return NoContent();
        }
    }
}