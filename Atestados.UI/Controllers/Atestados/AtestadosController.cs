using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Atestados.Datos.Modelo;
using Atestados.Negocios.Negocios;
using Atestados.Objetos.Dtos;
using Newtonsoft.Json;

namespace Atestados.UI.Controllers
{
    public class AtestadosController : Controller
    {
        private AtestadosEntities db = new AtestadosEntities();
        private InformacionAtestado infoAtestado = new InformacionAtestado();

        // GET: Atestados
        public ActionResult Index()
        {
            return View(infoAtestado.CargarAtestados());
        }

        // GET: Atestados/Ver/5
        public ActionResult Ver(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AtestadoDTO atestado = infoAtestado.CargarAtestado(id);
            if (atestado == null)
            {
                return HttpNotFound();
            }
            return View(atestado);
        }

        // GET: Atestados/Crear
        public ActionResult Crear()
        {
            AtestadoDTO atestado = new AtestadoDTO();
            atestado.FechaFinal = DateTime.Now;
            atestado.FechaInicio = DateTime.Now;
            atestado.NumeroAutores = 1;
            if (Session["Archivos"] == null)
            {
                Session["Archivos"] = new List<ArchivoDTO>();
            }

            ViewBag.PaisID = new SelectList(db.Pais, "PaisID", "Nombre", infoAtestado.ObtenerIDdePais("costa rica"));
            ViewBag.RubroID = new SelectList(db.Rubro, "RubroID", "Nombre");
            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersona((int)Session["UsuarioID"]);
            return View(atestado);
        }

        // POST: Atestados/Crear
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear([Bind(Include = "AtestadoID,Nombre,Archivos,NumeroAutores,Observaciones,HoraCreacion,Enviado,Descargado,CantidadHoras,Lugar,CatalogoTipo,Enlace,PaisID,PersonaID,RubroID")] AtestadoDTO atestado)
        {
            if (ModelState.IsValid)
            {
                atestado.PersonaID = (int)Session["UsuarioID"]; // cambiar por sesion
                Atestado a = AutoMapper.Mapper.Map<AtestadoDTO, Atestado>(atestado);
                infoAtestado.GuardarAtestado(a);
                atestado.AtestadoID = a.AtestadoID;
                Fecha fecha = AutoMapper.Mapper.Map<AtestadoDTO, Fecha>(atestado);
                infoAtestado.GuardarFecha(fecha);

                List<ArchivoDTO> archivos = (List<ArchivoDTO>)Session["Archivos"];
                foreach (ArchivoDTO archivo in archivos)
                {
                    Archivo ar = AutoMapper.Mapper.Map<ArchivoDTO, Archivo>(archivo);
                    ar.AtestadoID = a.AtestadoID;
                    infoAtestado.GuardarArchivo(ar);
                }

                Session["Archivos"] = new List<ArchivoDTO>();

                return RedirectToAction("Crear");
            }

            ViewBag.PaisID = new SelectList(db.Pais, "PaisID", "Nombre", atestado.PaisID);
            ViewBag.RubroID = new SelectList(db.Rubro, "RubroID", "Nombre", atestado.RubroID);
            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersona((int)Session["UsuarioID"]);
            return View(atestado);
        }

        // GET: Atestados/Editar/5
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (Session["Archivos"] == null)
            {
                Session["Archivos"] = new List<ArchivoDTO>();
            }

            Atestado atestado = infoAtestado.CargarAtestadoParaEditar(id);
            AtestadoDTO a = AutoMapper.Mapper.Map<Atestado, AtestadoDTO>(atestado);
            if (atestado == null)
            {
                return HttpNotFound();
            }
            ViewBag.PaisID = new SelectList(db.Pais, "PaisID", "Nombre", infoAtestado.ObtenerIDdePais("costa rica"));
            ViewBag.RubroID = new SelectList(db.Rubro, "RubroID", "Nombre", atestado.RubroID);
            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersona((int)Session["UsuarioID"]);
            return View(a);
        }

        // POST: Atestados/Editar/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "Annio,Archivos,AtestadoID,AtestadoXPersona,Editorial,Enlace,HoraCreacion,Nombre,NumeroAutores,Observaciones,PaisID,Persona,PersonaID,RubroID,Website,Fecha,DominioIdioma,Persona,Rubro,AtestadoXPersona,Pais,InfoEditorial,Archivo")] AtestadoDTO atestado)
        {
            if (ModelState.IsValid)
            {
                atestado.PersonaID = (int)Session["UsuarioID"]; // cambiar por sesion
                atestado.Fecha.FechaID = atestado.AtestadoID;
                atestado.Fecha.FechaInicio = DateTime.Now;
                infoAtestado.EditarFecha(AutoMapper.Mapper.Map<FechaDTO, Fecha>(atestado.Fecha));
                atestado.HoraCreacion = DateTime.Now;
                atestado.Archivos = infoAtestado.CargarArchivosDeAtestado(atestado.AtestadoID);
                infoAtestado.EditarAtestado(AutoMapper.Mapper.Map<AtestadoDTO, Atestado>(atestado));

                List<ArchivoDTO> archivos = (List<ArchivoDTO>)Session["Archivos"];
                foreach (ArchivoDTO archivo in archivos)
                {
                    Archivo ar = AutoMapper.Mapper.Map<ArchivoDTO, Archivo>(archivo);
                    ar.AtestadoID = atestado.AtestadoID;
                    infoAtestado.GuardarArchivo(ar);
                }

                Session["Archivos"] = new List<ArchivoDTO>();

                return RedirectToAction("Crear");
            }

            ViewBag.PaisID = new SelectList(db.Pais, "PaisID", "Nombre", infoAtestado.ObtenerIDdePais("costa rica"));
            ViewBag.RubroID = new SelectList(db.Rubro, "RubroID", "Nombre");
            ViewBag.Atestados = infoAtestado.CargarAtestadosDePersona((int)Session["UsuarioID"]);
            return View(atestado);
        }

        // GET: Libro/Borrar
        public ActionResult Borrar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Atestado atestado = infoAtestado.CargarAtestadoParaBorrar(id);
            if (atestado == null)
            {
                return HttpNotFound();
            }
            return View(atestado);
        }

        // POST: Libro/Borrar
        [HttpPost, ActionName("Borrar")]
        [ValidateAntiForgeryToken]
        public ActionResult Borrar(int id)
        {
            infoAtestado.BorrarAtestado(id);
            return RedirectToAction("Crear");
        }

        [HttpPost]
        public JsonResult Cargar(HttpPostedFileBase archivo)
        {
            byte[] bytes;
            using (BinaryReader br = new BinaryReader(archivo.InputStream))
            {
                bytes = br.ReadBytes(archivo.ContentLength);
            }

            if (Session["Archivos"] == null)
            {
                Session["Archivos"] = new List<ArchivoDTO>();
            }

            ArchivoDTO ar = new ArchivoDTO
            {
                Nombre = Path.GetFileName(archivo.FileName),
                TipoArchivo = archivo.ContentType,
                Datos = bytes
            };
            List<ArchivoDTO> archivos = (List<ArchivoDTO>)Session["Archivos"];
            archivos.Add(ar);
            Session["Archivos"] = archivos;

            var jsonTest = JsonConvert.SerializeObject(ar);

            return Json(new
            {
                archivoJson = jsonTest
            });
        }

        [HttpPost]
        public FileResult Descargar(int? archivoID)
        {
            ArchivoDTO archivo = infoAtestado.CargarArchivo(archivoID);
            return File(archivo.Datos, archivo.TipoArchivo, archivo.Nombre);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
