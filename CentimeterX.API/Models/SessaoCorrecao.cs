using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CentimeterX.API.Models.Enums;

namespace CentimeterX.API.Models
{
    [Table("CX_SESSAO_CORRECAO")]
    public class SessaoCorrecao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_SESSAO")]
        public int IdSessao { get; set; }

        [Column("ID_ROVER")]
        public int IdRover { get; set; }

        [ForeignKey("IdRover")]
        public Rover? Rover { get; set; }

        [Column("ID_ESTACAO_BASE")]
        public int IdEstacaoBase { get; set; }

        [ForeignKey("IdEstacaoBase")]
        public EstacaoBase? EstacaoBase { get; set; }

        [Column("TP_STATUS_FIX")]
        public StatusFix StatusFix { get; set; } = StatusFix.SINGLE;

        [Required]
        [Column("NR_PRECISAO_HORIZONTAL_CM")]
        public double PrecisaoHorizontalCm { get; set; }

        [Required]
        [Column("NR_PRECISAO_VERTICAL_CM")]
        public double PrecisaoVerticalCm { get; set; }

        [StringLength(50)]
        [Column("DS_SISTEMA_SATELITE")]
        public string SistemaSatelite { get; set; } = string.Empty;

        [Column("DT_INICIOU_EM")]
        public DateTime IniciouEm { get; set; } = DateTime.UtcNow;

        [Column("DT_ENCERRADO_EM")]
        public DateTime? EncerradoEm { get; set; }
    }
}