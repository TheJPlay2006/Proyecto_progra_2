using System.Collections.Generic;

namespace SistemaDeTickets.Controlador.Patrones
{
    public abstract class SujetoObservable
    {
        private readonly List<IObservador> observadores = new List<IObservador>();

        public void AgregarObservador(IObservador observador)
        {
            if (!observadores.Contains(observador))
                observadores.Add(observador);
        }

        public void EliminarObservador(IObservador observador)
        {
            if (observadores.Contains(observador))
                observadores.Remove(observador);
        }

        protected void NotificarObservadores(TipoNotificacion tipo, object datos)
        {
            foreach (var obs in observadores)
            {
                obs.Actualizar(tipo, datos);
            }
        }
    }
}
