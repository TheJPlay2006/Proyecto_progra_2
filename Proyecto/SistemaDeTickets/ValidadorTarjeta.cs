using System.Text.RegularExpressions;

namespace SistemaDeTickets.Controlador
{
    public static class ValidadorTarjeta
    {
        public static bool ValidarNumero(string numero)
        {
            return Regex.IsMatch(numero, @"^\d{16}$");
        }

        public static bool ValidarCVV(string cvv)
        {
            return Regex.IsMatch(cvv, @"^\d{3,4}$");
        }

        public static bool ValidarFechaExpiracion(string fecha)
        {
            return Regex.IsMatch(fecha, @"^(0[1-9]|1[0-2])\/\d{2}$");
        }

        public static bool ValidarNombreTitular(string nombre)
        {
            return !string.IsNullOrWhiteSpace(nombre);
        }
    }
}
