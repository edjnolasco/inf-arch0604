using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INF._5120.Arch0604.Domain.Entities
{
    public class Country
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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
        public bool IsEnable { get; set; } = true;

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}