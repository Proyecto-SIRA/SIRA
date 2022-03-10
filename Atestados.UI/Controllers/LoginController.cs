using Atestados.Negocios.Negocios;
using Atestados.Objetos.Clases;
using Atestados.Objetos.Dtos;
using Atestados.UIProcess.Clases;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace Atestados.UI.Controllers
{
    public class LoginController : Controller
    {
        #region Instancias
        //Instancia a la clase de Servicios en UIProcess
        private AccesoSeguridad ServiciosSeguridad = new AccesoSeguridad();
        //Objeto de retorno
        oRespuesta<string> respuestaValidacion = new oRespuesta<string>();

        //Debido que el usuario aun no está logueado, se usa un dummy inicial al logueo.
        //Luego de loguearse, cambia los datos al usuario que se encuentra logueado.
        int idUsuarioLogueado = 1;
        string usuarioLogueado = "sistemaLogueo@itcr.ac.cr";
        public string sessionId = ConfigurationManager.AppSettings["SessionID"];
        private InformacionGeneral info = new InformacionGeneral();

        public void InitializeController(RequestContext context)
        {
            base.Initialize(context);
        }
        #endregion

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string email, string contrasena)
        {
            if (ModelState.IsValid)
            {
                if (info.ValidarUsuario(email, contrasena)) {
                    UsuarioDTO usuario = info.UsuarioPorEmail(email);
                    Session["Usuario"] = usuario;
                    ViewBag.NombreUsuario = usuario.Email;
                    ViewBag.NombreCompleto = usuario.NombreCompleto();
                    Session["UsuarioID"] = usuario.UsuarioID;
                    Session["TipoUsuario"] = usuario.TipoUsuario;
                    return RedirectToAction("Index", "Funcionario");
                } 
                return View();
            }
            else
            {
                return View();
            }
        }

        public ActionResult PaginaInvalida()
        {
            return View();
        }

        //Cerrar sesión de aplicación
        public ActionResult CerrarSesion()
        {
            //inserta en bitácora del sistema de seguridad la salida
            if (Session["Usuario"] != null)
            {
                // ServiciosSeguridad.InsertarBitacoraSistema("Cierre de sesión Usuario : " + Session[ConfigurationManager.AppSettings["UsuarioLogueado"]].ToString(), Constantes.CodigosBitacora.INACTIVAR, "LoginController - Interno", "CerrarSesion", false, Convert.ToInt32(Session[ConfigurationManager.AppSettings["CodigoUsuarioLogueado"]]), Session[ConfigurationManager.AppSettings["UsuarioLogueado"]].ToString(), Utilitarios.Clases.Utilitarios.GetIpAddress(), Session[sessionId].ToString(), null);
            }



            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            Session.Clear();
            Session.Abandon();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));

            return RedirectToAction("Index", "Login");
        }
    }
}