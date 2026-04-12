using System.ComponentModel.DataAnnotations;

namespace INF._5120.Arch0604.Application.DTOs.CountryDTOs
{
    public class CreateCountryRequestDto
    {
        [Required]
        [StringLength(255)]
        public required string Description { get; set; }

        [Required]
        public int IsoNum { get; set; }

        [Required]
        [StringLength(2)]
        public required string IsoA2 { get; set; }

        [Required]
        [StringLength(3)]
        public required string IsoA3 { get; set; }

        [Required]
        public bool IsEnable { get; set; }
    }
}
