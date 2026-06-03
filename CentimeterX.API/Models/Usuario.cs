using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CentimeterX.API.Models.Enums;

namespace CentimeterX.API.Models
{
    [Table("CX_USUARIO")]
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }

        [Required]
        [StringLength(100)]
        [Column("NM_USUARIO")]
        public string Nome { get; set; }

        [Required]
        [StringLength(150)]
        [Column("DS_EMAIL")]
        public string Email { get; set; }

        [Column("TP_PERFIL")]
        public PerfilUsuario Perfil { get; set; } = PerfilUsuario.Operador;

        [Column("DT_CRIADO_EM")]
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    }
}