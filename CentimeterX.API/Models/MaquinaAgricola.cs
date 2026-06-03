using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentimeterX.API.Models
{
    public class MaquinaAgricola : Rover
    {
        [Required]
        [StringLength(100)]
        [Column("NM_MODELO_TRATOR")]
        public string ModeloTrator { get; set; }

        [Column("NR_LARGURA_IMPLEMENTO")]
        public decimal LarguraImplemento { get; set; }
    }
}