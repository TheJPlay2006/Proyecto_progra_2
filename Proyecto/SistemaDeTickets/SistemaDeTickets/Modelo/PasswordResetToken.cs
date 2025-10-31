using System;

namespace SistemaDeTickets.Modelo
{
    /// <summary>
    /// Modelo para tokens de recuperación de contraseña
    /// </summary>
    public class PasswordResetToken
    {
        /// <summary>
        /// Token único para recuperación (32 bytes en Base64)
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// ID del usuario al que pertenece el token
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Email del usuario (para validación adicional)
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Fecha y hora de creación del token
        /// </summary>
        public DateTime FechaCreacion { get; set; }

        /// <summary>
        /// Fecha y hora de expiración (30 minutos después de creación)
        /// </summary>
        public DateTime FechaExpiracion { get; set; }

        /// <summary>
        /// Indica si el token ya fue usado
        /// </summary>
        public bool Usado { get; set; }

        /// <summary>
        /// Constructor para crear un nuevo token
        /// </summary>
        public PasswordResetToken(int usuarioId, string email, string token)
        {
            UsuarioId = usuarioId;
            Email = email;
            Token = token;
            FechaCreacion = DateTime.Now;
            FechaExpiracion = FechaCreacion.AddMinutes(30);
            Usado = false;
        }

        /// <summary>
        /// Verifica si el token está válido (no expirado y no usado)
        /// </summary>
        public bool EsValido()
        {
            return !Usado && DateTime.Now <= FechaExpiracion;
        }

        /// <summary>
        /// Marca el token como usado
        /// </summary>
        public void MarcarComoUsado()
        {
            Usado = true;
        }

        /// <summary>
        /// Verifica si el token ha expirado
        /// </summary>
        public bool HaExpirado()
        {
            return DateTime.Now > FechaExpiracion;
        }
    }
}