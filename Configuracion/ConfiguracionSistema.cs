// Clase encargada de cargar y representar la configuración general del sistema
// desde el archivo appsettings.json (rutas, puerto y estado de sincronización)


using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using TareaCorta1.Red;

namespace TareaCorta1.Configuracion
{
    public class ConfiguracionSistema
    {
        // Ruta de la carpeta que será monitoreada para detectar nuevos archivos
        public string RutaCarpetaSincronizada { get; set; }

        // Ruta donde se guardarán los archivos replicados recibidos
        public string RutaCarpetaReplicada { get; set; }

        // Puerto de comunicación para sockets TCP
        public int Puerto { get; set; }

        // Indica si la sincronización está habilitada o no
        public bool SincronizacionHabilitada { get; set; }

        public List<Equipo> Equipos { get; set; }

        // Carga la configuración desde el archivo appsettings.json
        public static ConfiguracionSistema Cargar()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            var config = builder.Build();

            return config.Get<ConfiguracionSistema>();
        }
    }
}