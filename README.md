# 📁 Tarea Corta – Réplica de Archivos entre Equipos

## 👤 Estudiantes

**Aguilar Rojas, Felipe**
**Calderón Vega, Andrey**
**Garbanzo Picado, Jared**
**Gutiérrez Chaves, Jesús**
**Pacheco Coto, Gerald**

## 📚 Curso
Programación V – Tecnologías de la Información  
Colegio Universitario de Cartago  
Cuatrimestre I – 2026

---

## 🎯 Objetivo del Proyecto
Desarrollar una solución que permita **replicar automáticamente archivos** entre diferentes equipos de una red local, de forma similar al funcionamiento de servicios como *Dropbox*, garantizando que los cambios realizados en una carpeta se reflejen en otra.

---

## 📝 Descripción General
El sistema permite definir una **carpeta origen** y una **carpeta destino** mediante un archivo de configuración.  
Cuando un archivo es agregado, modificado o eliminado en la carpeta origen, el sistema detecta el cambio y replica la acción en la carpeta destino.

La solución puede ejecutarse:
- En **una sola computadora** (replicación local).
- En **dos computadoras distintas** dentro de la misma red.

Cada equipo mantiene **una sola carpeta**, y ambas se sincronizan entre sí.

---

## ⚙️ Funcionalidades Principales
- 📂 Monitoreo automático de una carpeta configurada.
- 🔁 Replicación de archivos en tiempo real.
- 🌐 Soporte para ejecución en red local.
- 🛠️ Configuración externa mediante `appsettings.json`.
- 💻 Compatible con sistemas Windows y Linux.

---

## 🧱 Estructura del Proyecto
TareaCorta_ReplicaArchivos
├─ Core
├─ UI
├─ Red
├─ Modelos
├─ Program.cs
├─ TareaCorta1.sln
├─ TareaCorta1.csproj
├─ appsettings.json
└─ .gitignore


---

## 🛠️ Tecnologías Utilizadas
- **Lenguaje:** C#
- **Plataforma:** .NET
- **Tipo de aplicación:** Consola
- **Control de versiones:** Git / GitHub

---

## 🔧 Configuración del Sistema
El archivo `appsettings.json` permite definir:

```json
{
  "DireccionIp": "IP_DEL_EQUIPO",
  "RutaCarpetaSincronizada": "Ruta de la carpeta origen",
  "RutaCarpetaReplicada": "Ruta de la carpeta destino"
}
🔹 Escenarios posibles
Misma PC: ambas rutas apuntan a carpetas locales.

PCs distintas: cada equipo define su propia carpeta y la IP del otro.

▶️ Cómo Ejecutar el Proyecto
Clonar el repositorio:

git clone https://github.com/andreyac1397/TareaCorta_ReplicaArchivos.git
Abrir el proyecto en Visual Studio.

Configurar correctamente el archivo appsettings.json.

Ejecutar la aplicación.

Agregar o modificar archivos en la carpeta origen.

Verificar que los cambios se repliquen en la carpeta destino.

✅ Resultado Esperado
Los archivos agregados, modificados o eliminados en la carpeta origen
se replican automáticamente en la carpeta destino.

La sincronización funciona sin intervención manual del usuario.

📌 Observaciones
El proyecto está orientado a demostrar el concepto de replicación de archivos.

No se utiliza interfaz gráfica, ya que el enfoque es funcional y lógico, ademas de la compatibilidad con linux es mejor y esta pensado que sea para diferentes sietemas operativos.

La estructura del repositorio fue diseñada para mantener claridad y buenas prácticas.

📎 Repositorio
🔗 Repositorio en GitHub

🏁 Conclusión
La solución desarrollada cumple con los criterios solicitados, permitiendo la sincronización eficiente de archivos entre equipos, aplicando conceptos de monitoreo de directorios, configuración externa y comunicación básica en red.
