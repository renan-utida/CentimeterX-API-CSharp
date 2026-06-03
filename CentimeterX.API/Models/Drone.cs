using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentimeterX.API.Models
{
    public class Drone : Rover
    {
        [Column("NR_AUTONOMIA_VOO")]
        public int AutonomiaVoo { get; set; }

        [Column("NR_ALTITUDE_MAXIMA")]
        public double AltitudeMaxima { get; set; }
    }
}