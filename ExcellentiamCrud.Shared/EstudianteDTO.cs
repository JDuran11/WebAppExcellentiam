using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ExcellentiamCrud.Shared
{
    public class EstudianteDTO
    {
        public int EstudianteId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Apellido { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string CorreoElectronico { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string? Direccion { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public DateTime? FechaRegistro { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public bool? Activo { get; set; }

        [Required]
        [Range(1,int.MaxValue, ErrorMessage = "El campo {0} es requerido")]
        public int? CursoId { get; set; }

        public CursoDTO? Curso { get; set; }
    }
}
