using BCrypt.Net;

namespace SistemaDeTickets.Utils
{
    public static class HashContrasena
    {
        public static string CrearHash(string contrasena)
        {
            return BCrypt.Net.BCrypt.HashPassword(contrasena, workFactor: 11);
        }

        public static bool VerificarHash(string contrasena, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(contrasena, hash);
        }
    }
}
