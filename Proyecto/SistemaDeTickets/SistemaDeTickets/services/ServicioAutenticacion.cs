using SistemaDeTickets.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeTickets.Services
{
    public static class ServicioAutenticacion
    {
    // Usuario actual (null = no logeado)
        public static Usuario CurrentUser { get; private set; } = null;

    // Inicia sesión (devuelve true si ok)
    public static bool Login(string email, string password)
    {
        // Aquí puedes usar tu servicio/archivo usuarios.json para validar.
        // Ejemplo mínimo: buscar en usuarios.json
        var repo = new ServicioUsuario(); // crea este servicio que lee usuarios.json
        var usuario = repo.ObtenerPorCorreo(email);
        if (usuario != null && usuario.PasswordHash == password) // compara (si tienes hash, compara hash)
        {
            CurrentUser = usuario;
            return true;
        }
        return false;
    }

    public static void Logout()
    {
        CurrentUser = null;
    }

    public static bool IsLoggedIn()
    {
        return CurrentUser != null;
    }
  }
}
    