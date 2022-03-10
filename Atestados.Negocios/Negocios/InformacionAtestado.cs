using Atestados.Datos.Modelo;
using Atestados.Objetos.Dtos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atestados.Negocios.Negocios
{
    public class InformacionAtestado
    {
        private AtestadosEntities db = new AtestadosEntities();
        private InformacionGeneral info = new InformacionGeneral();

        #region Atestado

        public int ObtenerIDdeRubro(string rubro) {
            List<Rubro> rubros = db.Rubro.Where(l => l.Nombre.Contains(rubro.Trim().ToLower())).ToList();
            return rubros.Count > 0 ? rubros.FirstOrDefault().RubroID : -1;
        }

        public string ObtenerRubro(int id)
        {
            return db.Rubro.Where(r => r.RubroID == id).FirstOrDefault().Nombre;
        }

        public List<AtestadoDTO> CargarAtestadosDePersonaEnviados(int personaID, int v)
        {
            List<Atestado> listaAtestado = db.Atestado.Where(a => a.PersonaID == personaID && a.Enviado == v).ToList();

            List<AtestadoDTO> listaAtestadosDto = AutoMapper.Mapper.Map<List<Atestado>, List<AtestadoDTO>>(listaAtestado);

            return listaAtestadosDto;
        }

        public int ObtenerIDdePais(string pais)
        {
            List<Pais> rubros = db.Pais.Where(l => l.Nombre.Contains(pais.Trim().ToLower())).ToList();
            return rubros.Count > 0 ? rubros.FirstOrDefault().PaisID : -1;
        }

        public AtestadoDTO CargarAtestado(int? id)
        {
            Atestado atestado = db.Atestado.Find(id);

            if (atestado == null)
                return null;

            AtestadoDTO atestadosDto = AutoMapper.Mapper.Map<Atestado, AtestadoDTO>(atestado);

            return atestadosDto;

        }

        public Atestado CargarAtestadoParaEditar(int? id)
        {
            Atestado atestado = db.Atestado.Find(id);

            if (atestado == null)
                return null;

            return atestado;

        }

        public Atestado CargarAtestadoParaBorrar(int? id)
        {
            Atestado atestado = db.Atestado.Find(id);

            if (atestado == null)
                return null;

            return atestado;

        }

        public List<AtestadoDTO> CargarAtestados()
        {

            List<Atestado> listaAtestado = db.Atestado.Include(a => a.Pais).Include(a => a.Persona).Include(a => a.Rubro).Include(a => a.DominioIdioma).Include(a => a.Fecha).Include(a => a.InfoEditorial).ToList();

            List<AtestadoDTO> listaAtestadosDto = AutoMapper.Mapper.Map<List<Atestado>, List<AtestadoDTO>>(listaAtestado);

            return listaAtestadosDto;

        }

        public List<AtestadoDTO> CargarAtestadosDeTipo(int? rubroId)
        {

            List<Atestado> listaAtestado = db.Atestado.Where(l => l.RubroID == rubroId).ToList();

            List<AtestadoDTO> listaAtestadosDto = AutoMapper.Mapper.Map<List<Atestado>, List<AtestadoDTO>>(listaAtestado);

            return listaAtestadosDto;

        }

        public List<AtestadoDTO> CargarAtestadosDePersona(int? personaID)
        {

            List<Atestado> listaAtestado = db.Atestado.Where(a => a.PersonaID == personaID).ToList();

            List<AtestadoDTO> listaAtestadosDto = AutoMapper.Mapper.Map<List<Atestado>, List<AtestadoDTO>>(listaAtestado);

            return listaAtestadosDto;

        }

        public List<AtestadoDTO> CargarAtestadosDePersonaPorTipo(int? rubroId, int? personaID)
        {

            List<Atestado> listaAtestado = db.Atestado.Where(a => a.RubroID == rubroId && a.PersonaID == personaID).ToList();

            List<AtestadoDTO> listaAtestadosDto = AutoMapper.Mapper.Map<List<Atestado>, List<AtestadoDTO>>(listaAtestado);

            return listaAtestadosDto;

        }


        public void GuardarAtestado(Atestado atestado)
        {
            db.Atestado.Add(atestado);
            db.SaveChanges();
        }

        public void EditarAtestado(Atestado atestado)
        {
            //db.Entry(atestado).State = EntityState.Modified;
            db.Set<Atestado>().AddOrUpdate(atestado);
            db.SaveChanges();
        }

        public void BorrarAtestado(int id)
        {
            Atestado atestado = db.Atestado.Find(id);
            db.Atestado.Remove(atestado);
            db.SaveChanges();
        }

        public List<Atestado> ObtenerAtestadosEnviados()
        {
            List<Atestado> query = db.Atestado.Where(a => a.Enviado == 1).ToList();
            return query;
        }

        public List<EnviadoDTO> PersonasEntregaron()
        {
            List<Atestado> enviados = ObtenerAtestadosEnviados();
            Dictionary<int, List<Atestado>> temp = new Dictionary<int, List<Atestado>>();

            foreach (Atestado atestado in enviados)
            {
                if(!temp.ContainsKey(atestado.PersonaID))
                {
                    temp.Add(atestado.PersonaID, new List<Atestado>());
                }

                temp[atestado.PersonaID].Add(atestado);
            }

            return temp.Select(kvp => new EnviadoDTO() { 
                PersonaID = kvp.Key,
                Atestados = kvp.Value,
                Persona = info.CargarPersona(kvp.Key)
            }).ToList();
        }

        public EnviadoDTO ObtenerEnviado(int personaID)
        {
            List<EnviadoDTO> enviados = PersonasEntregaron();
            return enviados.FirstOrDefault(x => x.PersonaID == personaID);
        }

        #endregion

        #region InfoEditorial
        public InfoEditorialDTO CargarInfoEditorial(int? id)
        {
            InfoEditorial infoEditorial = db.InfoEditorial.Find(id);

            if (infoEditorial == null)
                return null;

            InfoEditorialDTO infoEditorialsDto = AutoMapper.Mapper.Map<InfoEditorial, InfoEditorialDTO>(infoEditorial);

            return infoEditorialsDto;

        }

        public InfoEditorial CargarInfoEditorialParaEditar(int? id)
        {
            InfoEditorial infoEditorial = db.InfoEditorial.Find(id);

            if (infoEditorial == null)
                return null;

            return infoEditorial;

        }

        public InfoEditorial CargarInfoEditorialParaBorrar(int? id)
        {
            InfoEditorial infoEditorial = db.InfoEditorial.Find(id);

            if (infoEditorial == null)
                return null;

            return infoEditorial;

        }

        public List<InfoEditorialDTO> CargarInfoEditoriales()
        {

            List<InfoEditorial> listaInfoEditorial = db.InfoEditorial.Include(i => i.Atestado).ToList();

            List<InfoEditorialDTO> listaInfoEditorialsDto = AutoMapper.Mapper.Map<List<InfoEditorial>, List<InfoEditorialDTO>>(listaInfoEditorial);

            return listaInfoEditorialsDto;

        }

        public void GuardarInfoEditorial(InfoEditorial infoEditorial)
        {
            db.InfoEditorial.Add(infoEditorial);
            db.SaveChanges();
        }

        public void EditarInfoEditorial(InfoEditorial infoEditorial)
        {
            db.Entry(infoEditorial).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void BorrarInfoEditorial(int id)
        {
            InfoEditorial infoEditorial = db.InfoEditorial.Find(id);
            db.InfoEditorial.Remove(infoEditorial);
            db.SaveChanges();
        }
        #endregion

        #region Fecha
        public FechaDTO CargarFecha(int? id)
        {
            Fecha fecha = db.Fecha.Find(id);

            if (fecha == null)
                return null;

            FechaDTO fechasDto = AutoMapper.Mapper.Map<Fecha, FechaDTO>(fecha);

            return fechasDto;

        }

        public Fecha CargarFechaParaEditar(int? id)
        {
            Fecha fecha = db.Fecha.Find(id);

            if (fecha == null)
                return null;

            return fecha;

        }

        public Fecha CargarFechaParaBorrar(int? id)
        {
            Fecha fecha = db.Fecha.Find(id);

            if (fecha == null)
                return null;

            return fecha;

        }

        public List<FechaDTO> CargarFechas()
        {

            List<Fecha> listaFecha = db.Fecha.Include(f => f.Atestado).ToList();

            List<FechaDTO> listaFechasDto = AutoMapper.Mapper.Map<List<Fecha>, List<FechaDTO>>(listaFecha);

            return listaFechasDto;

        }

        public void GuardarFecha(Fecha fecha)
        {
            db.Fecha.Add(fecha);
            db.SaveChanges();
        }

        public void EditarFecha(Fecha fecha)
        {
            db.Entry(fecha).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void BorrarFecha(int id)
        {
            Fecha fecha = db.Fecha.Find(id);
            db.Fecha.Remove(fecha);
            db.SaveChanges();
        }
        #endregion

        #region Archivo

        public void BorrarArchivos(int? id)
        {
            db.Archivo.RemoveRange(db.Archivo.Where(x => x.AtestadoID == id));
            db.SaveChanges();
        }

        public ArchivoDTO CargarArchivo(int? id)
        {
            Archivo archivo = db.Archivo.Find(id);

            if (archivo == null)
                return null;

            ArchivoDTO archivosDto = AutoMapper.Mapper.Map<Archivo, ArchivoDTO>(archivo);

            return archivosDto;

        }

        public Archivo CargarArchivoParaEditar(int? id)
        {
            Archivo archivo = db.Archivo.Find(id);

            if (archivo == null)
                return null;

            return archivo;

        }

        public Archivo CargarArchivoParaBorrar(int? id)
        {
            Archivo archivo = db.Archivo.Find(id);

            if (archivo == null)
                return null;

            return archivo;

        }

        public List<ArchivoDTO> CargarArchivos()
        {

            List<Archivo> listaArchivo = db.Archivo.ToList();

            List<ArchivoDTO> listaArchivosDto = AutoMapper.Mapper.Map<List<Archivo>, List<ArchivoDTO>>(listaArchivo);

            return listaArchivosDto;

        }

        public List<ArchivoDTO> CargarArchivosDeAtestado(int? id)
        {

            List<Archivo> listaArchivo = db.Archivo.Where(archivo => archivo.AtestadoID == id).ToList();

            List<ArchivoDTO> listaArchivosDto = AutoMapper.Mapper.Map<List<Archivo>, List<ArchivoDTO>>(listaArchivo);

            return listaArchivosDto;

        }

        public void GuardarArchivo(Archivo archivo)
        {
            db.Archivo.Add(archivo);
            db.SaveChanges();
        }

        public void EditarArchivo(Archivo archivo)
        {
            db.Entry(archivo).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void BorrarArchivo(int id)
        {
            Archivo archivo = db.Archivo.Find(id);
            db.Archivo.Remove(archivo);
            db.SaveChanges();
        }
        #endregion

        #region AtestadoXPersona

        public List<AutorDTO> CargarAutoresAtestado(int? id)
        {
            var query = db.Persona
                    .Join(db.AtestadoXPersona,
                          m => m.PersonaID,
                          v => v.PersonaID,
                          (m, v) => new { m, v })
                    .Where(x => x.v.AtestadoID == id)
                    .Select(x => new AutorDTO() { 
                        Nombre = x.m.Nombre,
                        PrimerApellido = x.m.PrimerApellido,
                        SegundoApellido = x.m.SegundoApellido,
                        Email = x.m.Email,
                        Porcentaje = (double)x.v.Porcentaje,
                        PersonaID = x.m.PersonaID,
                        AtestadoID = x.v.AtestadoID,
                        Departamento = x.v.Departamento
                    });

            return query.ToList();
        }

        public List<AtestadoXPersona> CargarAtestadoXPersonasdeAtestado(int? id)
        {
            return db.AtestadoXPersona.Where(x => x.AtestadoID == id).ToList();
        }

        public AtestadoXPersonaDTO CargarAtestadoXPersona(int? id)
        {
            AtestadoXPersona atestadoXPersona = db.AtestadoXPersona.Find(id);

            if (atestadoXPersona == null)
                return null;

            AtestadoXPersonaDTO atestadoXPersonasDto = AutoMapper.Mapper.Map<AtestadoXPersona, AtestadoXPersonaDTO>(atestadoXPersona);

            return atestadoXPersonasDto;

        }

        public AtestadoXPersona CargarAtestadoXPersonaParaEditar(int? id)
        {
            AtestadoXPersona atestadoXPersona = db.AtestadoXPersona.Find(id);

            if (atestadoXPersona == null)
                return null;

            return atestadoXPersona;

        }

        public AtestadoXPersona CargarAtestadoXPersonaParaBorrar(int? id)
        {
            AtestadoXPersona atestadoXPersona = db.AtestadoXPersona.Find(id);

            if (atestadoXPersona == null)
                return null;

            return atestadoXPersona;

        }

        public List<AtestadoXPersonaDTO> CargarAtestadoXPersonas()
        {

            List<AtestadoXPersona> listaAtestadoXPersona = db.AtestadoXPersona.Include(a => a.Atestado).Include(a => a.Persona).ToList();

            List<AtestadoXPersonaDTO> listaAtestadoXPersonasDto = AutoMapper.Mapper.Map<List<AtestadoXPersona>, List<AtestadoXPersonaDTO>>(listaAtestadoXPersona);

            return listaAtestadoXPersonasDto;

        }

        public void GuardarAtestadoXPersona(AtestadoXPersona atestadoXPersona)
        {
            db.AtestadoXPersona.Add(atestadoXPersona);
            db.SaveChanges();
        }

        public void EditarAtestadoXPersona(AtestadoXPersona atestadoXPersona)
        {
            db.Entry(atestadoXPersona).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void BorrarAtestadoXPersona(int id)
        {
            AtestadoXPersona atestadoXPersona = db.AtestadoXPersona.Find(id);
            db.AtestadoXPersona.Remove(atestadoXPersona);
            db.SaveChanges();
        }

        #endregion

        #region DominioIdioma

        public int ObtenerIdioma(string nombre)
        {
            List<Idioma> idioma = db.Idioma.Where(l => l.Nombre.Contains(nombre.Trim().ToLower())).ToList();
            return idioma.Count > 0 ? idioma.FirstOrDefault().IdiomaID : -1;
        }

        public DominioIdiomaDTO CargarDominioIdioma(int? id)
        {
            DominioIdioma dominioIdioma = db.DominioIdioma.Find(id);

            if (dominioIdioma == null)
                return null;

            DominioIdiomaDTO dominioIdiomasDto = AutoMapper.Mapper.Map<DominioIdioma, DominioIdiomaDTO>(dominioIdioma);

            return dominioIdiomasDto;

        }

        public DominioIdioma CargarDominioIdiomaParaEditar(int? id)
        {
            DominioIdioma dominioIdioma = db.DominioIdioma.Find(id);

            if (dominioIdioma == null)
                return null;

            return dominioIdioma;

        }

        public DominioIdioma CargarDominioIdiomaParaBorrar(int? id)
        {
            DominioIdioma dominioIdioma = db.DominioIdioma.Find(id);

            if (dominioIdioma == null)
                return null;

            return dominioIdioma;

        }

        public List<DominioIdiomaDTO> CargarDominioIdiomas()
        {

            List<DominioIdioma> listaDominioIdioma = db.DominioIdioma.Include(a => a.Atestado).Include(a => a.Idioma).ToList();

            List<DominioIdiomaDTO> listaDominioIdiomasDto = AutoMapper.Mapper.Map<List<DominioIdioma>, List<DominioIdiomaDTO>>(listaDominioIdioma);

            return listaDominioIdiomasDto;

        }

        public void GuardarDominioIdioma(DominioIdioma dominioIdioma)
        {
            db.DominioIdioma.Add(dominioIdioma);
            db.SaveChanges();
        }

        public void EditarDominioIdioma(DominioIdioma dominioIdioma)
        {
            db.Entry(dominioIdioma).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void BorrarDominioIdioma(int id)
        {
            DominioIdioma dominioIdioma = db.DominioIdioma.Find(id);
            db.DominioIdioma.Remove(dominioIdioma);
            db.SaveChanges();
        }

        #endregion
    }
}
