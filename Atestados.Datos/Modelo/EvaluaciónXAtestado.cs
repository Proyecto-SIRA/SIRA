//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Atestados.Datos.Modelo
{
    using System;
    using System.Collections.Generic;
    
    public partial class EvaluaciónXAtestado
    {
        public int AtestadoID { get; set; }
        public int PersonaID { get; set; }
        public double PorcentajeObtenido { get; set; }
        public string Observaciones { get; set; }
    
        public virtual Atestado Atestado { get; set; }
        public virtual Persona Persona { get; set; }
    }
}
