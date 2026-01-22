using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TareaCorta1.Monitoreo;

namespace TareaCorta1.Red
{
    public class ServidorArchivos
    {
        private readonly int puerto;
        private readonly string rutaSync;          // ahora es la MISMA carpeta sincronizada
        private readonly MonitorCarpeta monitor;   // para suspender eventos

        private TcpListener listener;
        private bool activo;

        public ServidorArchivos(int puerto, string rutaSync, MonitorCarpeta monitor)
        {
            this.puerto = puerto;
            this.rutaSync = rutaSync;
            this.monitor = monitor;
        }

        public void Iniciar()
        {
            if (activo) return;
            activo = true;

            listener = new TcpListener(IPAddress.Any, puerto);
            listener.Start();

            Thread hilo = new Thread(EscucharClientes) { IsBackground = true };
            hilo.Start();
        }

        public void Detener()
        {
            activo = false;
            try { listener?.Stop(); } catch { }
        }

        private void EscucharClientes()
        {
            while (activo)
            {
                try
                {
                    TcpClient cliente = listener.AcceptTcpClient();
                    new Thread(() => RecibirArchivo(cliente)) { IsBackground = true }.Start();
                }
                catch
                {
                    // cuando hacemos Stop(), AcceptTcpClient lanza excepción: salimos si ya no está activo
                    if (!activo) return;
                }
            }
        }

        private void RecibirArchivo(TcpClient cliente)
        {
            using (cliente)
            using (NetworkStream stream = cliente.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            {
                string nombreArchivo = reader.ReadString();
                int tamano = reader.ReadInt32();
                byte[] datos = reader.ReadBytes(tamano);

                string rutaFinal = Path.Combine(rutaSync, nombreArchivo);

                // ✅ Anti-loop: suspendemos watcher mientras escribimos el archivo recibido
                monitor.SuspenderEventos();
                try
                {
                    File.WriteAllBytes(rutaFinal, datos);
                }
                finally
                {
                    monitor.ReanudarEventos();
                }
            }
        }
    }
}
