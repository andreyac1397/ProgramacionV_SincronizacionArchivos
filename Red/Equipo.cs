// Representa un equipo remoto dentro del sistema de sincronización


namespace TareaCorta1.Red
{
    public class Equipo
    {
        // Nombre identificador del equipo
        public string NombreEquipo { get; set; }

        // Dirección IP del equipo remoto
        public string DireccionIP { get; set; }

        public int Puerto { get; set; }


        // Indica si la sincronización está pausada
        public bool SincronizacionPausada { get; set; }

        // Pausa la sincronización del equipo
        public void PausarSincronizacion () => SincronizacionPausada = true;

        // Reanuda la sincronización del equipo
        public void ReanudarSincronizacion () => SincronizacionPausada = false;


        // Indica si el equipo está disponible para sincronizar
        public bool EstadoDisponible() => !SincronizacionPausada;
    }
}
