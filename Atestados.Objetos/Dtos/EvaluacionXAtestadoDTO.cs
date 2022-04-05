using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atestados.Objetos.Dtos
{
    public class EvaluacionXAtestadoDTO
    {
        public int AtestadoID { get; set; }
        public int PersonaID { get; set; }
        public float PorcentajeObtenido { get; set; }
        public string Observaciones { get; set; }

    }
}
