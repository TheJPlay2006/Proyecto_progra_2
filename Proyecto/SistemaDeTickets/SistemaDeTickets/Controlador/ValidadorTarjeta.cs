using System;
using System.Text.RegularExpressions;

namespace SistemaDeTickets.Controlador
{
    /// <summary>
    /// Tipos de tarjetas soportados
    /// </summary>
    public enum TipoTarjeta
    {
        Visa,
        Mastercard,
        AmericanExpress,
        Desconocida
    }

    /// <summary>
    /// Información de validación de tarjeta
    /// </summary>
    public class ValidacionTarjeta
    {
        public bool Valida { get; set; }
        public string Mensaje { get; set; }
        public TipoTarjeta Tipo { get; set; }
        public string Marca { get; set; }
    }

    public static class ValidadorTarjeta
    {
        /// <summary>
        /// Implementa el algoritmo de Luhn para validar el número de tarjeta
        /// </summary>
        public static bool ValidarLuhn(string numeroTarjeta)
        {
            if (string.IsNullOrWhiteSpace(numeroTarjeta))
                return false;

            // Solo dígitos
            string soloDigitos = Regex.Replace(numeroTarjeta, @"[^\d]", "");
            
            if (soloDigitos.Length < 13 || soloDigitos.Length > 19)
                return false;

            int suma = 0;
            bool esPar = false;

            // Procesar de derecha a izquierda
            for (int i = soloDigitos.Length - 1; i >= 0; i--)
            {
                int digito = int.Parse(soloDigitos[i].ToString());

                if (esPar)
                {
                    digito *= 2;
                    if (digito > 9)
                        digito -= 9;
                }

                suma += digito;
                esPar = !esPar;
            }

            return (suma % 10) == 0;
        }

        /// <summary>
        /// Detecta la marca de la tarjeta por BIN (Bank Identification Number)
        /// </summary>
        public static TipoTarjeta DetectarMarca(string numeroTarjeta)
        {
            if (string.IsNullOrWhiteSpace(numeroTarjeta))
                return TipoTarjeta.Desconocida;

            string soloDigitos = Regex.Replace(numeroTarjeta, @"[^\d]", "");

            // Visa: comienza con 4
            if (soloDigitos.StartsWith("4") && soloDigitos.Length >= 13 && soloDigitos.Length <= 19)
                return TipoTarjeta.Visa;

            // Mastercard: comienza con 51-55 o 2221-2720
            if ((soloDigitos.StartsWith("51") || soloDigitos.StartsWith("52") || soloDigitos.StartsWith("53") ||
                 soloDigitos.StartsWith("54") || soloDigitos.StartsWith("55") ||
                 (soloDigitos.StartsWith("2221") || soloDigitos.StartsWith("2720")))
                 && soloDigitos.Length == 16)
                return TipoTarjeta.Mastercard;

            // American Express: comienza con 34 o 37
            if ((soloDigitos.StartsWith("34") || soloDigitos.StartsWith("37")) && soloDigitos.Length == 15)
                return TipoTarjeta.AmericanExpress;

            return TipoTarjeta.Desconocida;
        }

        /// <summary>
        /// Valida el CVV según el tipo de tarjeta
        /// </summary>
        public static bool ValidarCVV(string cvv, TipoTarjeta tipoTarjeta)
        {
            if (string.IsNullOrWhiteSpace(cvv))
                return false;

            switch (tipoTarjeta)
            {
                case TipoTarjeta.AmericanExpress:
                    return Regex.IsMatch(cvv, @"^\d{4}$");
                case TipoTarjeta.Visa:
                case TipoTarjeta.Mastercard:
                    return Regex.IsMatch(cvv, @"^\d{3}$");
                default:
                    return Regex.IsMatch(cvv, @"^\d{3,4}$");
            }
        }

        /// <summary>
        /// Valida la fecha de expiración en formato MM/YY (método privado interno)
        /// </summary>
        private static bool ValidacionFechaExpiracion(string fecha)
        {
            if (string.IsNullOrWhiteSpace(fecha))
                return false;

            // Formato MM/YY
            var match = Regex.Match(fecha, @"^(0[1-9]|1[0-2])\/(\d{2})$");
            if (!match.Success)
                return false;

            int mes = int.Parse(match.Groups[1].Value);
            int año = int.Parse("20" + match.Groups[2].Value);

            DateTime fechaExpiracion = new DateTime(año, mes, 1);
            fechaExpiracion = fechaExpiracion.AddMonths(1).AddDays(-1); // Último día del mes

            // La tarjeta es válida hasta el último día del mes de expiración
            return fechaExpiracion >= DateTime.Today;
        }

        /// <summary>
        /// Valida el nombre del titular (método privado interno)
        /// </summary>
        private static bool ValidacionNombreTitular(string nombre)
        {
            return !string.IsNullOrWhiteSpace(nombre) && nombre.Trim().Length >= 2;
        }

        /// <summary>
        /// Validación completa de tarjeta con información detallada
        /// </summary>
        public static ValidacionTarjeta ValidarTarjetaCompleta(string numero, string cvv, string fecha, string nombreTitular)
        {
            var validacion = new ValidacionTarjeta();

            // Validar número de tarjeta con Luhn
            if (!ValidarLuhn(numero))
            {
                validacion.Valida = false;
                validacion.Mensaje = "Número de tarjeta inválido (falla algoritmo de Luhn)";
                return validacion;
            }

            // Detectar marca
            TipoTarjeta tipo = DetectarMarca(numero);
            validacion.Tipo = tipo;
            validacion.Marca = tipo.ToString();

            if (tipo == TipoTarjeta.Desconocida)
            {
                validacion.Valida = false;
                validacion.Mensaje = "Marca de tarjeta no soportada";
                return validacion;
            }

            // Validar CVV según tipo
            if (!ValidarCVV(cvv, tipo))
            {
                validacion.Valida = false;
                validacion.Mensaje = $"CVV inválido para {tipo}";
                return validacion;
            }

            // Validar fecha
            if (!ValidacionFechaExpiracion(fecha))
            {
                validacion.Valida = false;
                validacion.Mensaje = "Fecha de expiración inválida o vencida";
                return validacion;
            }

            // Validar nombre
            if (!ValidacionNombreTitular(nombreTitular))
            {
                validacion.Valida = false;
                validacion.Mensaje = "Nombre del titular inválido";
                return validacion;
            }

            validacion.Valida = true;
            validacion.Mensaje = "Tarjeta válida";
            return validacion;
        }

        /// <summary>
        /// Métodos legacy para compatibilidad
        /// </summary>
        public static bool ValidarNumero(string numero)
        {
            return ValidarLuhn(numero);
        }

        public static bool ValidarCVV(string cvv)
        {
            return ValidarCVV(cvv, TipoTarjeta.Desconocida);
        }

        public static bool ValidarFechaExpiracion(string fecha)
        {
            return ValidacionFechaExpiracion(fecha);
        }

        public static bool ValidarNombreTitular(string nombre)
        {
            return ValidacionNombreTitular(nombre);
        }
    }
}
