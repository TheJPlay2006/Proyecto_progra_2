using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeTickets.Modelo
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }
        public string Rol { get; set; } = "usuario";

        public Usuario() { }
        public Usuario(int id, string nombre, string correo, string contraseña, string rol)
        {
            Id = id;
            Nombre = nombre;
            Correo = correo;
            Contraseña = contraseña;
            Rol = rol;
        }
    }
}
