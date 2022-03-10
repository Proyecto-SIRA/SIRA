using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atestados.Objetos.Dtos
{
    public class ArchivoDTO
    {
        public int ArchivoID { get; set; }
        public bool Obligatorio { get; set; }
        public string Nombre { get; set; }
        public byte[] Datos { get; set; }
        public string TipoArchivo { get; set; }
        public int AtestadoID { get; set; }

    }
}
