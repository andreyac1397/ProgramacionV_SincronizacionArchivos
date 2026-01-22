using System;
using TareaCorta1.Configuracion;
using TareaCorta1.Core;
using UI;

class Program
{
    static void Main()
    {
        // 1) Carga configuración inicial
        var config = ConfiguracionSistema.Cargar();

        // 2) Verifica equipos configurados (opcional, puedes permitir correr sin equipos para pruebas)
        if (config.Equipos == null || config.Equipos.Count == 0)
        {
            Console.WriteLine("No hay equipos configurados para replicación.");
            // Puedes retornar o dejar que el usuario abra el menú y cambie config
            // return;
        }

        // 3) Inicializa el sistema (se inicia/detiene desde el menú)
        var sistema = new SistemaReplicacion(config);

        // 4) Menú (HU3)
        var menu = new MenuConfiguracion(sistema, config);
        menu.Mostrar();
    }
}
