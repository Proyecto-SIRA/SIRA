using Atestados.Datos.Modelo;
using Atestados.Negocios.Negocios;
using Atestados.Objetos.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IronXL;
using System.Text;

namespace Atestados.UI.Controllers
{
    public class ComisionController : Controller
    {
        private readonly InformacionAtestado infoAtestado = new InformacionAtestado();

        public ActionResult Index()
        {
            List<EnviadoDTO> enviados = infoAtestado.PersonasEntregaron();
            return View(enviados);
        }

        public FileResult Descarga(int id)
        {
            EnviadoDTO enviado = infoAtestado.ObtenerEnviado(id);
            List<Archivo> archivos = new List<Archivo>();
            foreach (Atestado atestado in enviado.Atestados)
            {
                archivos.AddRange(atestado.Archivo.ToList());
            }
            byte[] file = GetZipArchive(archivos);
            return File(file, "application/zip", $"{enviado.Persona.Nombre}_{enviado.Persona.PrimerApellido}_{enviado.Persona.SegundoApellido}.zip");
        }

        public FileResult GenerarExcel(int id)
        {
            EnviadoDTO enviado = infoAtestado.ObtenerEnviado(id);
            var csv = new StringBuilder();
            var linea = $"Nombre;Rubro;Enlace;Observaciones";
            csv.AppendLine(linea);
            foreach (Atestado atestado in enviado.Atestados)
            {
                linea = $"{atestado.Nombre};{infoAtestado.ObtenerRubro(atestado.RubroID)};{atestado.Enlace};{atestado.Observaciones}";
                csv.AppendLine(linea);
                foreach (Archivo archivo in atestado.Archivo)
                {
                    linea = $";{archivo.Nombre}";
                    csv.AppendLine(linea);
                }
            }

            return File(Encoding.Unicode.GetBytes(csv.ToString()), "text/csv", $"{enviado.Persona.Nombre}_{enviado.Persona.PrimerApellido}_{enviado.Persona.SegundoApellido}.csv");
        }

        private static byte[] GetZipArchive(List<Archivo> files)
        {
            byte[] archiveFile;
            using (var archiveStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in files)
                    {
                        var zipArchiveEntry = archive.CreateEntry(file.Nombre, CompressionLevel.Fastest);
                        using (var zipStream = zipArchiveEntry.Open())
                            zipStream.Write(file.Datos, 0, file.Datos.Length);
                    }
                }

                archiveFile = archiveStream.ToArray();
            }

            return archiveFile;
        }
    }
}