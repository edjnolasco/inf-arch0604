using System.ComponentModel.DataAnnotations;

namespace INF._5120.Arch0604.Application.DTOs.CountryDTOs
{
    public class CreateCountryRequestDto
    {
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public required string Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "IsoNum debe ser mayor que cero.")]
        public int IsoNum { get; set; }

        [Required]
        [StringLength(2, MinimumLength = 2)]
        public required string IsoA2 { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public required string IsoA3 { get; set; }

        public bool IsEnable { get; set; }
    }
}