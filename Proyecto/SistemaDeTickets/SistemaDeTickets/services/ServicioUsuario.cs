﻿using Newtonsoft.Json;
using SistemaDeTickets.Modelo;
using SistemaDeTickets.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeTickets.Services
{
    internal class ServicioUsuario
    {
        private string path = "Data/Usuarios.json";

        public ServicioUsuario()
        {
            // Nota: Migración automática removida para evitar StackOverflowException
            // La migración se debe llamar explícitamente desde el punto de entrada de la aplicación
        }

        public List<Usuario> ObtenerTodos()
        {
            return GestorJSON.LeerArchivo<List<Usuario>>(path) ?? new List<Usuario>();
        }

        public Usuario ObtenerPorCorreo(string email)
        {
            var usuarios = ObtenerTodos();
            Console.WriteLine($"[DEBUG SERVICIOUSUARIO] Total usuarios cargados: {usuarios.Count}");
            foreach (var u in usuarios)
            {
                Console.WriteLine($"[DEBUG SERVICIOUSUARIO] Usuario: {u.Email} (Rol: {u.Rol})");
            }

            var usuarioEncontrado = usuarios.FirstOrDefault(u => u.Email.Equals(email, System.StringComparison.OrdinalIgnoreCase));
            Console.WriteLine($"[DEBUG SERVICIOUSUARIO] Buscando email: '{email}', encontrado: {usuarioEncontrado != null}");

            return usuarioEncontrado;
        }

        /// <summary>
        /// Guarda todos los usuarios usando escritura atómica
        /// </summary>
        public void GuardarTodos(List<Usuario> usuarios)
        {
            GestorJSON.EscribirAtomico(path, usuarios);
        }
    }
}
