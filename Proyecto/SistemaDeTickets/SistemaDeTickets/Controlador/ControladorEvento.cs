using Newtonsoft.Json;
using SistemaDeTickets.Modelo;
using SistemaDeTickets.Controlador.Patrones;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeTickets.Controlador
{
    internal class ControladorEvento
    {
        // Ruta del archivo JSON donde se guardan los eventos
        private string path = "Data/MisEventos.json";

        // Lista donde se van a guardar los eventos que se leen del JSON
        private List<Modelo.Evento> eventos;

        // Constructor: cuando se crea este controlador, se cargan los eventos del archivo
        public ControladorEvento()
        {
            eventos = LeerEventos();
        }

        // Leer los eventos del archivo JSON
       
        private List<Modelo.Evento> LeerEventos()
        {
            // Si el archivo no existe, se devuelve una lista vacía
            if (!File.Exists(path)) return new List<Modelo.Evento>();

            // Lee todo el texto del archivo JSON
            string json = File.ReadAllText(path);

            // Convierte ese texto en una lista de objetos tipo Evento
            return JsonConvert.DeserializeObject<List<Modelo.Evento>>(json);
        }

        // Guardar la lista de eventos en el archivo JSON
       
        private void GuardarEventos()
        {
            // Convierte la lista de eventos a texto JSON y la escribe en el archivo
            File.WriteAllText(path, JsonConvert.SerializeObject(eventos, Formatting.Indented));
        }

        // Obtener todos los eventos (sin filtros)
        
        public List<Modelo.Evento> ObtenerEventos()
        {
            // Devuelve la lista completa de eventos
            return eventos;
        }

        
        // Filtrar eventos (forma normal)
       
        public List<Modelo.Evento> FiltrarEventos(string nombre, string tipo, string recinto)
        {
            var filtrados = eventos;

            // Si se escribe algo en el filtro de nombre, busca por nombre
            if (!string.IsNullOrEmpty(nombre))
                filtrados = filtrados.FindAll(e => e.Nombre.ToLower().Contains(nombre.ToLower()));

            // Si se escribe algo en el filtro de tipo, busca por tipo
            if (!string.IsNullOrEmpty(tipo))
                filtrados = filtrados.FindAll(e => e.Tipo.ToLower().Contains(tipo.ToLower()));

            // Si se escribe algo en el filtro de recinto, busca por recinto
            if (!string.IsNullOrEmpty(recinto))
                filtrados = filtrados.FindAll(e => e.Recinto.ToLower().Contains(recinto.ToLower()));

            // Devuelve la lista filtrada
            return filtrados;
        }

        
        // Filtrar eventos (usando recursividad)
       
        public List<Modelo.Evento> FiltrarEventosRecursivo(string nombre, string tipo, string recinto)
        {
            // Llama al método recursivo que hace el filtrado
            return FiltrarRecursivo(eventos, nombre?.ToLower(), tipo?.ToLower(), recinto?.ToLower(), 0, new List<Modelo.Evento>());
        }

        // Método que se llama a sí mismo para recorrer la lista
        private List<Modelo.Evento> FiltrarRecursivo(List<Modelo.Evento> lista, string nombre, string tipo, string recinto, int index, List<Modelo.Evento> resultados)
        {
            // Si ya llegó al final de la lista, devuelve los resultados encontrados
            if (index >= lista.Count) return resultados;

            // Toma el evento actual
            Modelo.Evento e = lista[index];
            bool coincide = true;

            // Revisa si cumple con los filtros
            if (!string.IsNullOrEmpty(nombre)) coincide &= e.Nombre.ToLower().Contains(nombre);
            if (!string.IsNullOrEmpty(tipo)) coincide &= e.Tipo.ToLower().Contains(tipo);
            if (!string.IsNullOrEmpty(recinto)) coincide &= e.Recinto.ToLower().Contains(recinto);

            // Si cumple con los filtros, se agrega a la lista de resultados
            if (coincide) resultados.Add(e);

            // Llama otra vez a la misma función pero avanzando al siguiente evento
            return FiltrarRecursivo(lista, nombre, tipo, recinto, index + 1, resultados);
        }

        
        // Comprar tiquetes (resta la cantidad y guarda los cambios)
        
        public bool Comprar(Modelo.Evento eventoSeleccionado, int cantidad = 1)
        {
            // Busca el evento por su ID
            var e = eventos.FirstOrDefault(ev => ev.Id == eventoSeleccionado.Id);

            // Si el evento existe y hay suficientes tiquetes disponibles
            if (e != null && e.TiquetesDisponibles >= cantidad)
            {
                // Resta la cantidad comprada
                e.TiquetesDisponibles -= cantidad;

                // Guarda la lista actualizada
                GuardarEventos();

                // Compra exitosa
                return true;
            }

            // Si no hay suficientes tiquetes o no se encontró el evento
            return false;
        }

        // Crear un nuevo evento (para el administrador)

        public void CrearEvento(Modelo.Evento nuevoEvento)
        {
            // Busca el ID más alto y le suma uno al nuevo evento
            int maxId = eventos.Count > 0 ? eventos.Max(e => e.Id) : 0;
            nuevoEvento.Id = maxId + 1;

            // Agrega el nuevo evento a la lista
            eventos.Add(nuevoEvento);

            // Guarda los cambios en el archivo
            GuardarEventos();

            // Notificar nuevo evento
            var gestorEventos = new GestorEventos();
            gestorEventos.NotificarNuevoEvento(nuevoEvento);
        }

        
        // Editar un evento existente
        
        public bool EditarEvento(Modelo.Evento eventoEditado)
        {
            // Busca el evento que tenga el mismo ID
            var e = eventos.FirstOrDefault(ev => ev.Id == eventoEditado.Id);

            // Si lo encuentra, actualiza sus datos
            if (e != null)
            {
                e.Nombre = eventoEditado.Nombre;
                e.Fecha = eventoEditado.Fecha;
                e.Recinto = eventoEditado.Recinto;
                e.Tipo = eventoEditado.Tipo;
                e.TiquetesDisponibles = eventoEditado.TiquetesDisponibles;
                e.Descripcion = eventoEditado.Descripcion;

                // Guarda la lista actualizada
                GuardarEventos();
                return true;
            }

            // Si no lo encuentra, devuelve falso
            return false;
        }

        
        // Eliminar un evento (por ID)
        
        public bool EliminarEvento(int id)
        {
            // Busca el evento con ese ID
            var e = eventos.FirstOrDefault(ev => ev.Id == id);

            // Si existe, lo elimina de la lista y guarda los cambios
            if (e != null)
            {
                eventos.Remove(e);
                GuardarEventos();
                return true;
            }

            // Si no se encontró el evento
            return false;
        }
    }
}