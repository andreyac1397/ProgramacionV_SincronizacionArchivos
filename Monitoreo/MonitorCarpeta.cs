// Clase encargada de monitorear una carpeta del sistema
// y detectar la creación/modificación de archivos para su replicación (modo espejo 1 carpeta)

using System;
using System.IO;
using System.Threading;
using TareaCorta1.Modelos;

namespace TareaCorta1.Monitoreo
{
    public class MonitorCarpeta
    {
        // Observador del sistema de archivos
        private readonly FileSystemWatcher watcher;

        // Hilo que mantiene activo el monitoreo
        private Thread hilo;

        // Indica si el monitoreo está activo
        private bool activo;

        // Controla eventos duplicados (Created + Changed)
        private DateTime ultimoEvento = DateTime.MinValue;

        // Control de suspensión (anti-loop al recibir por red)
        private readonly object _lockEventos = new object();
        private int _suspensiones = 0;

        // Delegado para notificar cuando se detecta un archivo
        public delegate void ArchivoDetectadoHandler(Archivo archivo);

        // Evento que se dispara al detectar un nuevo archivo válido
        public event ArchivoDetectadoHandler onArchivoDetectado;

        // Constructor que recibe la ruta a monitorear
        public MonitorCarpeta(string ruta)
        {
            watcher = new FileSystemWatcher(ruta)
            {
                Path = ruta,
                Filter = "*.*",
                IncludeSubdirectories = false,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size | NotifyFilters.LastWrite
            };

            watcher.Created += DetectarArchivo;
            watcher.Changed += DetectarArchivo;

            Console.WriteLine("[DEBUG] FileSystemWatcher creado para: " + ruta);
        }

        // Inicia el monitoreo de la carpeta
        public void Iniciar()
        {
            activo = true;
            watcher.EnableRaisingEvents = true;

            Console.WriteLine("[DEBUG] MonitorCarpeta INICIADO en: " + watcher.Path);

            hilo = new Thread(() =>
            {
                while (activo)
                {
                    Thread.Sleep(1000);
                }
            });

            hilo.IsBackground = true;
            hilo.Start();
        }

        // Detiene el monitoreo de la carpeta
        public void Detener()
        {
            activo = false;
            watcher.EnableRaisingEvents = false;
        }

        // Suspende temporalmente eventos del watcher (para escribir archivos recibidos por red sin loop)
        public void SuspenderEventos()
        {
            lock (_lockEventos)
            {
                _suspensiones++;
                watcher.EnableRaisingEvents = false;
            }
        }

        // Reanuda eventos del watcher (cuando termina de escribirse el archivo recibido)
        public void ReanudarEventos()
        {
            lock (_lockEventos)
            {
                _suspensiones--;
                if (_suspensiones <= 0)
                {
                    _suspensiones = 0;
                    if (activo) watcher.EnableRaisingEvents = true;
                }
            }
        }

        // Método que se ejecuta cuando se crea/modifica un archivo
        private void DetectarArchivo(object sender, FileSystemEventArgs e)
        {
            // ✅ Protección extra: si estamos recibiendo por red y hay un evento en cola, lo ignoramos
            lock (_lockEventos)
            {
                if (_suspensiones > 0) return;
            }

            // ✅ Filtro simple de duplicados (Created + Changed muy seguidos)
            if ((DateTime.Now - ultimoEvento).TotalMilliseconds < 500)
                return;

            ultimoEvento = DateTime.Now;

            try
            {
                string nombre = Path.GetFileName(e.FullPath);

                // Ignorar temporales
                if (nombre.StartsWith("~$") || nombre.EndsWith(".tmp"))
                    return;

                // Esperar a que el archivo esté completamente escrito
                EsperarArchivoLibre(e.FullPath);

                FileInfo info = new FileInfo(e.FullPath);

                // Reintento simple si el archivo aún está vacío
                int intentos = 0;
                while (info.Length == 0 && intentos < 5)
                {
                    Thread.Sleep(300);
                    info.Refresh();
                    intentos++;
                }

                if (info.Length == 0)
                    return;

                Archivo archivo = new Archivo
                {
                    Nombre = Path.GetFileNameWithoutExtension(info.Name),
                    Extension = info.Extension,
                    Ruta = info.FullName,
                    FechaCreacion = info.CreationTime,
                    Tamaño = info.Length
                };

                Console.WriteLine($"[INFO] Archivo detectado: {archivo.ObtenerNombreCompleto()}");

                onArchivoDetectado?.Invoke(archivo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] Error al detectar archivo: " + ex.Message);
            }
        }

        // Espera hasta que el archivo pueda abrirse sin bloqueo
        private void EsperarArchivoLibre(string ruta)
        {
            while (true)
            {
                try
                {
                    using (File.Open(ruta, FileMode.Open, FileAccess.Read, FileShare.None))
                        break;
                }
                catch
                {
                    Thread.Sleep(500);
                }
            }
        }
    }
}
