using Atestados.Datos.Modelo;
using Atestados.Negocios.Negocios;
using Atestados.Objetos.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Atestados.UI.Controllers
{
    public class FuncionarioController : Controller
    {
        private readonly InformacionAtestado infoAtestado = new InformacionAtestado();
        private readonly InformacionGeneral infoGeneral = new InformacionGeneral();

        // GET: Funcionario
        public ActionResult Index()
        {
            if (Session["Usuario"] != null)
            {
                return View(ObtenerPersona());
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }

        [HttpPost]
        public ActionResult Index(PersonaDTO persona)
        {
            foreach(AtestadoDTO a in persona.PorEnviar)
            {
                if (a.MarcarEnviado)
                {
                    Atestado atestado = infoAtestado.CargarAtestadoParaEditar(a.AtestadoID);
                    atestado.Enviado = a.MarcarEnviado ? 1 : 0;
                    infoAtestado.EditarAtestado(atestado);
                }      
            }
            return View(ObtenerPersona());
        }

        private PersonaDTO ObtenerPersona()
        {
            PersonaDTO persona = infoGeneral.CargarPersona((int)Session["UsuarioID"]);
            persona.PorEnviar = infoAtestado.CargarAtestadosDePersonaEnviados((int)Session["UsuarioID"], 0);
            ViewBag.Enviados = infoAtestado.CargarAtestadosDePersonaEnviados((int)Session["UsuarioID"], 1);
            ViewBag.Rubros = new Dictionary<int, string>()
                {
                    { infoAtestado.ObtenerIDdeRubro("Artículo"), "Articulo"},
                    { infoAtestado.ObtenerIDdeRubro(""), "Atestados"},
                    { infoAtestado.ObtenerIDdeRubro("Capacitación profesional"), "Capacitacion"},
                    { infoAtestado.ObtenerIDdeRubro("Grados académicos"), "Certificado"},
                    { infoAtestado.ObtenerIDdeRubro("Idiomas"), "Idioma"},
                    { infoAtestado.ObtenerIDdeRubro("libro"), "Libro"},
                    { infoAtestado.ObtenerIDdeRubro("Obra administrativa de desarrollo"), "ObraAdministrativa"},
                    { infoAtestado.ObtenerIDdeRubro("Obra didáctica"), "ObraDidactica"},
                    { infoAtestado.ObtenerIDdeRubro("Ponencia"), "Ponencia"},
                    { infoAtestado.ObtenerIDdeRubro("Proyectos de investigación y extensión"), "Proyecto"},
                };
            return persona;
        }
    }
}