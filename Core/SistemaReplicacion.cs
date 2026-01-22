using System;
using System.IO;
using TareaCorta1.Monitoreo;
using TareaCorta1.Configuracion;
using TareaCorta1.Red;

namespace TareaCorta1.Core
{
    public class SistemaReplicacion
    {
        private MonitorCarpeta _monitor;
        private GestorArchivos _gestor;
        private ServidorArchivos _servidor;

        private readonly ConfiguracionSistema _config;
        private bool _activo;

        public SistemaReplicacion(ConfiguracionSistema config)
        {
            _config = config;
        }

        public void Iniciar()
        {
            if (_activo) throw new Exception("La sincronización ya está activa.");

            if (string.IsNullOrWhiteSpace(_config.RutaCarpetaSincronizada))
                throw new Exception("No se ha configurado la carpeta sincronizada.");

            if (!Directory.Exists(_config.RutaCarpetaSincronizada))
            {
                Exception ex = new DirectoryNotFoundException(
                    $"La carpeta sincronizada no existe: {_config.RutaCarpetaSincronizada}"
                );

                Logger.RegistrarError(ex);
                throw ex;
            }


            _monitor = new MonitorCarpeta(_config.RutaCarpetaSincronizada);
            _gestor = new GestorArchivos(_config.Equipos);


            _monitor.onArchivoDetectado += archivo => _gestor.ProcesarNuevoArchivo(archivo);

            // ✅ Servidor recibe en la MISMA carpeta y pausa el watcher al escribir
            _servidor = new ServidorArchivos(_config.Puerto, _config.RutaCarpetaSincronizada, _monitor);
            _servidor.Iniciar();

            _monitor.Iniciar();
            _activo = true;
        }

        public void Detener()
        {
            if (!_activo) throw new Exception("La sincronización ya está detenida.");

            _monitor.Detener();
            _servidor.Detener();

            _activo = false;
        }

        public bool EstaActivo() => _activo;
    }
}

