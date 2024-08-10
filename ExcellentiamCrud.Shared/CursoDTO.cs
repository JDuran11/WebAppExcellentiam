using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ExcellentiamCrud.Shared
{
    public class CursoDTO
    {
        public int CursoId { get; set; }

        public string NombreCurso { get; set; } = null!;

        public string? Descripcion { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }
    }
}
