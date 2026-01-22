using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using TareaCorta1.Modelos;
using TareaCorta1.Monitoreo;

namespace TareaCorta1.Red
{
    public class ReplicadorArchivos
    {
        public void Replicar(Archivo archivo, List<Equipo> equipos)
        {
            byte[] datos = File.ReadAllBytes(archivo.Ruta);
            string nombreCompleto = archivo.ObtenerNombreCompleto();

            foreach (var equipo in equipos)
            {
                if (!equipo.EstadoDisponible())
                    continue;

                try
                {
                    using (TcpClient cliente = new TcpClient(equipo.DireccionIP, equipo.Puerto))
                    using (NetworkStream stream = cliente.GetStream())
                    using (BinaryWriter writer = new BinaryWriter(stream))
                    {
                        writer.Write(nombreCompleto);
                        writer.Write(datos.Length);
                        writer.Write(datos);
                    }

                    // ✅ HU2: registro exitoso
                    Logger.RegistrarOperacion(nombreCompleto);
                }
                catch (System.Exception ex)
                {
                    // ✅ HU2: registro de error técnico
                    Logger.RegistrarError(ex);
                }
            }
        }
    }
}