using System;
using System.IO;

namespace TareaCorta1.Monitoreo
{
    public static class Logger
    {
        // Archivo bitácora (queda junto al .exe)
        private static readonly string logPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bitacora.txt");

        // Registrar operación exitosa
        public static void RegistrarOperacion(string nombreArchivo)
        {
            try
            {
                string usuario = Environment.UserName;
                string fecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                string linea =
                    $"[{fecha}] Usuario: {usuario} - Se ha agregado el nuevo archivo {nombreArchivo}";

                Escribir(linea);

                // ✅ Mensaje visible para pruebas
                Console.WriteLine($"[LOG] Bitácora registrada: {nombreArchivo}");
            }
            catch (Exception ex)
            {
                RegistrarError(ex);
            }
        }

        // Registrar error técnico
        public static void RegistrarError(Exception ex)
        {
            try
            {
                string fecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                string linea =
                    $"[{fecha}] ERROR TÉCNICO: {ex.Message} | {ex.StackTrace}";

                Escribir(linea);

                Console.WriteLine("[LOG] Error registrado en bitácora");
            }
            catch
            {
                // Nunca romper el sistema por la bitácora
            }
        }

        private static void Escribir(string linea)
        {
            using (StreamWriter sw = new StreamWriter(logPath, true))
            {
                sw.WriteLine(linea);
            }
        }
    }
}