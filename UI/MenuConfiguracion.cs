using System;
using System.IO;
using TareaCorta1.Core;
using TareaCorta1.Configuracion;

namespace UI
{
    public class MenuConfiguracion
    {
        private readonly SistemaReplicacion _sistema;
        private readonly ConfiguracionSistema _config;

        public MenuConfiguracion(SistemaReplicacion sistema, ConfiguracionSistema config)
        {
            _sistema = sistema;
            _config = config;
        }

        public void Mostrar()
        {
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                MostrarEncabezado();
                MostrarEstado();

                Console.WriteLine("1. Cambiar carpeta sincronizada (1 carpeta tipo espejo)");
                Console.WriteLine("2. Iniciar sincronización");
                Console.WriteLine("3. Detener sincronización");
                Console.WriteLine("4. Salir");
                Console.Write("\nSeleccione una opción: ");

                var opcion = Console.ReadLine();

                try
                {
                    switch (opcion)
                    {
                        case "1":
                            CambiarRutaSincronizada();
                            break;

                        case "2":
                            _sistema.Iniciar();
                            MensajeOk("Se ha iniciado la sincronización.");
                            break;

                        case "3":
                            _sistema.Detener();
                            MensajeOk("Se ha detenido la sincronización.");
                            break;

                        case "4":
                            salir = true;
                            break;

                        default:
                            MensajeError("Opción no válida.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MensajeError(ex.Message);
                }
            }
        }

        // =======================
        // CAMBIO DE RUTA (1 carpeta)
        // =======================

        private void CambiarRutaSincronizada()
        {
            Console.Write("\nIngrese la carpeta sincronizada (se enviará y recibirá aquí): ");
            var ruta = Console.ReadLine();

            if (!ValidarRuta(ruta)) return;

            _config.RutaCarpetaSincronizada = ruta;

            // En modo espejo, ya NO se usa RutaCarpetaReplicada.
            // La dejamos null/vacía para evitar confusión.
            _config.RutaCarpetaReplicada = null;

            MensajeOk("Carpeta sincronizada actualizada correctamente.");
        }

        // =======================
        // VALIDACIONES
        // =======================

        private bool ValidarRuta(string ruta)
        {
            if (string.IsNullOrWhiteSpace(ruta))
            {
                MensajeError("La ruta no puede estar vacía.");
                return false;
            }

            if (!Path.IsPathRooted(ruta))
            {
                MensajeError("Debe ingresar una ruta completa. Ejemplo: C:\\MisArchivos");
                return false;
            }

            if (!Directory.Exists(ruta))
            {
                MensajeError("La carpeta no existe. Debe crearla antes de usarla.");
                return false;
            }

            return true;
        }

        // =======================
        // UI
        // =======================

        private void MostrarEncabezado()
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("   SISTEMA DE SINCRONIZACIÓN DE ARCHIVOS");
            Console.WriteLine("        Pantalla de Configuración");
            Console.WriteLine("==============================================\n");
        }

        private void MostrarEstado()
        {
            Console.WriteLine("Estado de sincronización: " +
                (_sistema.EstaActivo() ? "ACTIVA" : "DETENIDA"));

            Console.WriteLine("Carpeta sincronizada: " +
                (_config.RutaCarpetaSincronizada ?? "(no definida)"));

            Console.WriteLine("----------------------------------------------\n");
        }

        private void MensajeOk(string mensaje)
        {
            Console.WriteLine("\n[OK] " + mensaje);
            Pausa();
        }

        private void MensajeError(string mensaje)
        {
            Console.WriteLine("\n[ERROR] " + mensaje);
            Pausa();
        }

        private void Pausa()
        {
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}
