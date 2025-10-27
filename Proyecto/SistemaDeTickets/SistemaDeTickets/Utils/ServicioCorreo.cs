using System;
using System.Net;
using System.Net.Mail;

namespace SistemaDeTickets.Utils
{
    /// <summary>
    /// Servicio para envío de correos electrónicos utilizando SMTP.
    /// Utilizado para recuperación de contraseña y notificaciones.
    /// </summary>
    public static class ServicioCorreo
    {
        // Configuración SMTP (ajustar según proveedor de correo)
        private const string SmtpHost = "smtp.gmail.com"; // Ejemplo: Gmail
        private const int SmtpPort = 587; // Puerto TLS
        private const string SmtpUser = "tuemail@gmail.com"; // Reemplazar con email real
        private const string SmtpPassword = "tucontraseña"; // Reemplazar con contraseña o app password

        /// <summary>
        /// Envía un correo electrónico con un token de recuperación.
        /// </summary>
        /// <param name="destinatario">Correo del destinatario</param>
        /// <param name="token">Token de recuperación generado</param>
        /// <returns>True si se envió correctamente, false en caso contrario</returns>
        public static bool EnviarTokenRecuperacion(string destinatario, string token)
        {
            try
            {
                using (var cliente = new SmtpClient(SmtpHost, SmtpPort))
                {
                    cliente.Credentials = new NetworkCredential(SmtpUser, SmtpPassword);
                    cliente.EnableSsl = true;

                    var mensaje = new MailMessage
                    {
                        From = new MailAddress(SmtpUser),
                        Subject = "Recuperación de Contraseña - Sistema de Tickets",
                        Body = $"Tu token de recuperación es: {token}\n\nEste token expira en 30 minutos.",
                        IsBodyHtml = false
                    };
                    mensaje.To.Add(destinatario);

                    cliente.Send(mensaje);
                    Logger.Info($"Token de recuperación enviado a {destinatario}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error al enviar correo de recuperación", ex);
                return false;
            }
        }

        /// <summary>
        /// Envía una notificación general por correo.
        /// </summary>
        /// <param name="destinatario">Correo del destinatario</param>
        /// <param name="asunto">Asunto del correo</param>
        /// <param name="cuerpo">Cuerpo del mensaje</param>
        /// <returns>True si se envió correctamente, false en caso contrario</returns>
        public static bool EnviarNotificacion(string destinatario, string asunto, string cuerpo)
        {
            try
            {
                using (var cliente = new SmtpClient(SmtpHost, SmtpPort))
                {
                    cliente.Credentials = new NetworkCredential(SmtpUser, SmtpPassword);
                    cliente.EnableSsl = true;

                    var mensaje = new MailMessage
                    {
                        From = new MailAddress(SmtpUser),
                        Subject = asunto,
                        Body = cuerpo,
                        IsBodyHtml = false
                    };
                    mensaje.To.Add(destinatario);

                    cliente.Send(mensaje);
                    Logger.Info($"Notificación enviada a {destinatario}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error al enviar notificación por correo", ex);
                return false;
            }
        }
    }
}