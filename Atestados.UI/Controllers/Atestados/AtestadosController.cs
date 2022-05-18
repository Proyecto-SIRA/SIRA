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
        private InformacionGeneral infoGeneral = new InformacionGeneral();

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

        // GET: Atestados/Evaluar
        public ActionResult Evaluar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UsuarioDTO usuario = (UsuarioDTO)Session["Usuario"];
            Session["TipoUsuario"] = usuario.TipoUsuario;
            Session["idAtestado"] = id;
            Session["idUsuario"] = usuario.UsuarioID;
            Session["autoresAtestado"] = infoAtestado.ObtenerAutores((int)id);


            List<EvaluaciónXAtestado> e = infoAtestado.ObtenerEvaluacionXAtestadoRevisor((int)id, usuario.UsuarioID);

            ViewBag.Revisor = infoGeneral.CargarPersona(usuario.UsuarioID);
            ViewBag.Atestado = infoAtestado.CargarAtestado(id);

            if (e != null)
            {
                //EvaluacionXAtestadoDTO edto = AutoMapper.Mapper.Map<EvaluaciónXAtestado, EvaluacionXAtestadoDTO>(e);
                //ViewBag.Evaluacion = edto;
                //return View(evaluacion);
            }

            AtestadoDTO atestado = infoAtestado.CargarAtestado(id);
            if (atestado == null)
            {
                return HttpNotFound();
            }
            EvaluacionXAtestadoDTO evaluacion = new EvaluacionXAtestadoDTO();
            evaluacion.Atestado = atestado;
            return View(evaluacion);
        }

        // evaluar
        // POST: Atestados/Evaluar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Evaluar([Bind(Include = "Observaciones")] EvaluacionXAtestadoDTO evaluacion)
        {
            if (ModelState.IsValid)
            {
                if (evaluacion.Observaciones == null) evaluacion.Observaciones = "N/A";

                evaluacion.AtestadoID = (int)Session["idAtestado"];
                evaluacion.PersonaID = (int)Session["idUsuario"];

                List<PersonaDTO> autores = (List<PersonaDTO>)Session["autoresAtestado"];
                List<EvaluacionXAtestadoDTO> puntos = (List<EvaluacionXAtestadoDTO>)Session["puntosAutores"];

                foreach (EvaluacionXAtestadoDTO eval in puntos)
                {
                    /*
                    float porcentaje = 0;
                    //int idAutor;
                    foreach (EvaluacionXAtestadoDTO eval in puntos)
                    {
                        if (eval.AutorID == autor.PersonaID)
                        {
                            porcentaje = eval.PorcentajeObtenido;
                        }
                    }
                    */

                    EvaluaciónXAtestado e = new EvaluaciónXAtestado()
                    {
                        AtestadoID = evaluacion.AtestadoID,
                        PersonaID = evaluacion.PersonaID,
                        AutorID = eval.AutorID,
                        PorcentajeObtenido = eval.PorcentajeObtenido,
                        Observaciones = evaluacion.Observaciones
                    };

                    /*
                    EvaluaciónXAtestado evaluacionActual = infoAtestado.ObtenerEvaluacionXAtestado((int)Session["idAtestado"], (int)Session["idUsuario"]);
                
                    if (evaluacionActual != null)
                    {
                        infoAtestado.BorrarEvaluacion((int)Session["idAtestado"], (int)Session["idUsuario"]);
                    }
                    */
                    db.EvaluaciónXAtestado.Add(e);


                    db.SaveChanges();
            
                }


                AtestadoDTO atestado = infoAtestado.CargarAtestado((int)Session["idAtestado"]);

                // Create dictionary of values
                Dictionary<string, string> values = new Dictionary<string, string>();
                values.Add("Libro", "Libro");
                values.Add("Artículo", "Articulo");
                values.Add("Obra didáctica", "ObraDidactica");
                values.Add("Obra administrativa de desarrollo", "ObraAdministrativa");

                String controlador = values[atestado.Rubro.Nombre];

                return RedirectToAction("Ver", controlador, new { id = (int)Session["idAtestado"] });
            }
            return View(evaluacion);
        }

        // POST: Atestados/ObtenerAutores
        [HttpPost]
        public JsonResult ObtenerAutores()
        {
            var autores = Session["autoresAtestado"];
            //var jsonTest = JsonConvert.SerializeObject(autores);
            return Json(autores);
        }

        // POST: Atestados/AsignarPuntos
        [HttpPost]
        public void AsignarPuntos(List<EvaluacionXAtestadoDTO> evaluacionData)
        {
            List<EvaluacionXAtestadoDTO> puntos = new List<EvaluacionXAtestadoDTO>();

            foreach (EvaluacionXAtestadoDTO evaluacion in evaluacionData) {

                EvaluacionXAtestadoDTO e = new EvaluacionXAtestadoDTO()
                {
                    AutorID = (int)evaluacion.AutorID,
                    PorcentajeObtenido = (int)evaluacion.PorcentajeObtenido
                };

                puntos.Add(e);

            }

            Session["puntosAutores"] = puntos;

            //Object[] autores = (Object[])Session["autoresAtestado"];

            //Object[] newautores; 
            /*
            foreach (Object autor in autores)
            {
                foreach (EvaluacionXAtestadoDTO e in json)
                {
                    if (e.PersonaID == autor.)
                }
                autor.PorcentajeObtenido
            }
            */
            //List<EvaluacionXAtestadoDTO> puntos = new List<EvaluacionXAtestadoDTO>();

        }
    }
}
