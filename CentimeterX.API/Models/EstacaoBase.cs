using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentimeterX.API.Models
{
    [Table("TB_ESTACAO_BASE")]
    public class EstacaoBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_ESTACAO")]
        public int IdEstacao { get; set; }

        [Required]
        [StringLength(20)]
        [Column("CD_CODIGO")]
        public string Codigo { get; set; }

        [Required]
        [StringLength(100)]
        [Column("NM_ESTACAO")]
        public string Nome { get; set; }

        [Required]
        [Column("NR_LATITUDE")]
        public double Latitude { get; set; }

        [Required]
        [Column("NR_LONGITUDE")]
        public double Longitude { get; set; }

        [Column("FL_ONLINE")]
        public bool Online { get; set; } = true;

        [StringLength(200)]
        [Column("DS_CONSTELACOES")]
        public string Constelacoes { get; set; } = string.Empty;

        [Column("DT_ULTIMA_ATUALIZACAO")]
        public DateTime UltimaAtualizacao { get; set; } = DateTime.UtcNow;
    }
}