using System;

namespace SistemaDeTickets.Modelo
{
    public class Inventario
    {
        public int EventoId { get; set; }
        public int TiquesDisponibles { get; set; }
        public int TiquesReservados { get; set; }
        public int TiquesVendidos { get; set; }

        public bool VerificarDisponibilidad(int cantidad)
        {
            return TiquesDisponibles >= cantidad;
        }

        public void ReservarTiques(int cantidad)
        {
            if (VerificarDisponibilidad(cantidad))
            {
                TiquesDisponibles -= cantidad;
                TiquesReservados += cantidad;
            }
        }

        public void ConfirmarVenta(int cantidad)
        {
            TiquesReservados -= cantidad;
            TiquesVendidos += cantidad;
        }

        public void LiberarReserva(int cantidad)
        {
            TiquesReservados -= cantidad;
            TiquesDisponibles += cantidad;
        }
    }
}
