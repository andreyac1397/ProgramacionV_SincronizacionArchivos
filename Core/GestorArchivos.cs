using System.Collections.Generic;
using System.IO;
using TareaCorta1.Modelos;
using TareaCorta1.Red;

namespace TareaCorta1.Core
{
    public class GestorArchivos
    {
        private readonly ReplicadorArchivos replicador;
        private readonly List<Equipo> equipos;

        public GestorArchivos(List<Equipo> equipos)
        {
            this.equipos = equipos;
            replicador = new ReplicadorArchivos(); // ✅ ya no recibe puerto
        }

        // Procesa el archivo detectado y lo replica por red
        public void ProcesarNuevoArchivo(Archivo archivo)
        {
            System.Console.WriteLine($"[INFO] Archivo detectado: {archivo.ObtenerNombreCompleto()}");

            // ✅ En modo espejo NO hacemos copia local a otra carpeta.
            // Solo replicamos a los otros equipos.
            replicador.Replicar(archivo, equipos);
        }
    }
}
