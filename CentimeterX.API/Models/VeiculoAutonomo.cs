using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentimeterX.API.Models
{
    public class VeiculoAutonomo : Rover
    {
        [Column("NR_NIVEL_AUTONOMIA")]
        public int NivelAutonomia { get; set; }

        [Column("NR_VELOCIDADE_MAXIMA")]
        public double VelocidadeMaxima { get; set; }
    }
}