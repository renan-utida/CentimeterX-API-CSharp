using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentimeterX.API.Models
{
    [Table("TB_ROVER")]
    public abstract class Rover
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_ROVER")]
        public int IdRover { get; set; }

        [Required]
        [StringLength(100)]
        [Column("NM_ROVER")]
        public string Nome { get; set; }

        [Column("TP_STATUS")]
        public StatusRover Status { get; set; } = StatusRover.Ativo;

        [Column("ID_ESTACAO_BASE")]
        public int IdEstacaoBase { get; set; }

        [ForeignKey("IdEstacaoBase")]
        public EstacaoBase? EstacaoBase { get; set; }

        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario? Usuario { get; set; }

        [Column("DT_CADASTRO")]
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    }
}