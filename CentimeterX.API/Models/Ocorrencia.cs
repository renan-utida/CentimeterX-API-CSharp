using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CentimeterX.API.Models.Enums;

namespace CentimeterX.API.Models
{
    [Table("TB_OCORRENCIA")]
    public class Ocorrencia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_OCORRENCIA")]
        public int IdOcorrencia { get; set; }

        [Column("ID_ROVER")]
        public int IdRover { get; set; }

        [ForeignKey("IdRover")]
        public Rover? Rover { get; set; }

        [Column("TP_OCORRENCIA")]
        public TipoOcorrencia Tipo { get; set; }

        [StringLength(500)]
        [Column("DS_DESCRICAO")]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        [Column("NR_LATITUDE")]
        public double Latitude { get; set; }

        [Required]
        [Column("NR_LONGITUDE")]
        public double Longitude { get; set; }

        [StringLength(500)]
        [Column("DS_FOTO_URL")]
        public string FotoUrl { get; set; } = string.Empty;

        [Column("DT_CRIADA_EM")]
        public DateTime CriadaEm { get; set; } = DateTime.UtcNow;
    }
}