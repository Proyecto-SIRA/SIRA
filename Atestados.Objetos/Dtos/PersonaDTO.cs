using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atestados.Objetos.Dtos
{
    public class PersonaDTO
    {
        public int PersonaID { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo inválido")]
        public string Email { get; set; }
        public int CategoriaActual { get; set; }
        public int TipoUsuario { get; set; }
        public int Telefono { get; set; }
        public List<AtestadoDTO> PorEnviar { get; set; }
    }

}
