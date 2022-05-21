using Atestados.Datos.Modelo;
using Atestados.Negocios.Negocios;
using Atestados.Objetos.Dtos;
using Atestados.Utilitarios.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Atestados.Objectos;

namespace Atestados.UI.Controllers
{
    public class AdministradorController : Controller
    {
        public AdministradorController()
        {
            var tiposUsuario = TiposHelper.ObtenerTiposUsuario();
            var tiposCategoria = TiposHelper.ObtenerTiposCategoria();
            ViewData["tiposUsuario"] = tiposUsuario;
            ViewData["tiposCategoria"] = tiposCategoria;
        }

        InformacionGeneral info = new InformacionGeneral();
        // GET: Administrador
        public ActionResult Index()
        {
            Session["TipoUsuarioNombre"] = "Admin";

            int adminID = TiposHelper.ObtenerTipoUsuarioID("Admin");

            if (Session["TipoUsuario"] != null && (int)Session["TipoUsuario"] == adminID) // Si es administrador
            {
                List<UsuarioDTO> usuario = info.ObtenerUsuarios((int)Session["UsuarioID"]);
                return View(usuario);
            }
            else
            {
                return RedirectToAction("Denegado");
            }
        }

        public ActionResult Guardar(int usuario, int tipo)
        {
            Persona persona = info.CargarPersonaParaEditar(usuario);
            persona.TipoUsuario = tipo;
            info.EditarPersona(persona);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Ver(int id)
        {
            List<PuntosXRubroDTO> puntosXRubroDTOs = new List<PuntosXRubroDTO>();

            PuntosXRubroDTO puntosXRubroDTO = new PuntosXRubroDTO();
            puntosXRubroDTO.Rubro = "Total";
            puntosXRubroDTO.PuntosPasoActual = 5;
            puntosXRubroDTO.PuntosMaximosPasoActual = 10;
            puntosXRubroDTO.PuntosAcumulados = 25;

            PuntosXRubroDTO puntosXRubroDTO2 = new PuntosXRubroDTO();
            puntosXRubroDTO2.Rubro = "Total 2";
            puntosXRubroDTO2.PuntosPasoActual = 6;
            puntosXRubroDTO2.PuntosMaximosPasoActual = 16;
            puntosXRubroDTO2.PuntosAcumulados = 23;

            puntosXRubroDTOs.Add(puntosXRubroDTO);
            puntosXRubroDTOs.Add(puntosXRubroDTO2);

            ViewBag.PuntosXRubroDTOs = puntosXRubroDTOs;

            return View(info.CargarPersona(id));
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            return View(info.CargarPersonaParaEditar(id));
        }

        [HttpPost]
        public ActionResult Editar(Persona persona)
        {
            persona.esActivo = true; //Asegurar que el usuario sigue activo.
            info.EditarPersona(persona);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Eliminar(int id)
        {
            return View(info.CargarPersonaParaBorrar(id));
        }

        [HttpPost]
        public ActionResult Eliminar(Persona persona)
        {
            info.BorrarPersona(persona.PersonaID);
            return RedirectToAction("Index");
        }

        public ActionResult Denegado()
        {
            return View();
        }
    }
}