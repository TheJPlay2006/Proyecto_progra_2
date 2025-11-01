namespace SistemaDeTickets.Modelo
{
    public enum RolUsuario
    {
        Usuario,
        Admin
    }

    public enum EstadoCompra
    {
        Pendiente,
        Procesando,
        Completada,
        Cancelada,
        Fallida
    }

    public enum MetodoPago
    {
        TarjetaCredito,
        TarjetaDebito
    }

    public enum TipoNotificacion
    {
        NuevoEvento,
        BajoInventario,
        CompraExitosa
    }

    public enum EstadoEvento
    {
        Activo,
        Inactivo,
        Cancelado
    }
}
