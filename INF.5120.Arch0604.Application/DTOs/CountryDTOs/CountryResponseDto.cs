namespace INF._5120.Arch0604.Application.DTOs.CountryDTOs
{
    public class CountryResponseDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public int IsoNum { get; set; }
        public string IsoA2 { get; set; } = string.Empty;
        public string IsoA3 { get; set; } = string.Empty;
        public bool IsEnable { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}