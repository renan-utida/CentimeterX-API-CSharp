namespace CentimeterX.API.Models
{
    /// <summary>
    /// Constantes do domínio GNSS utilizadas na classificação de precisão PPP.
    /// FIX: precisão centimétrica (≤ 5 cm)
    /// FLOAT: precisão decimétrica (≤ 50 cm)
    /// SINGLE: precisão métrica (> 50 cm)
    /// </summary>
    public static class GnssConstants
    {
        public const double FIX_THRESHOLD_CM = 5.0;
        public const double FLOAT_THRESHOLD_CM = 50.0;

        public const double LATITUDE_MIN = -90.0;
        public const double LATITUDE_MAX = 90.0;
        public const double LONGITUDE_MIN = -180.0;
        public const double LONGITUDE_MAX = 180.0;

        public const int STATUS_INATIVIDADE_HORAS = 24;
    }
}