// Modelo que representa un archivo del sistema
// Contiene sus datos básicos para ser replicado

using System;

namespace TareaCorta1.Modelos
{
    public class Archivo
    {
        // Nombre del archivo sin extensión
        public string Nombre { get; set; }

        // Extensión del archivo (ej: .txt, .pdf)
        public string Extension { get; set; }

        // Ruta completa donde se encuentra el archivo
        public string Ruta { get; set; }

        // Fecha de creación del archivo
        public DateTime FechaCreacion { get; set; }

        // Tamaño del archivo en bytes
        public long Tamaño { get; set; }


        // Retorna el nombre completo del archivo (nombre + extensión)
        public string ObtenerNombreCompleto()
        {
            return $"{Nombre}{Extension}";
        }
    }
}
