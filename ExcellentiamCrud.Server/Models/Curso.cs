using System;
using System.Collections.Generic;

namespace ExcellentiamCrud.Server.Models;

public partial class Curso
{
    public int CursoId { get; set; }

    public string NombreCurso { get; set; } = null!;

    public string? Descripcion { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }

    public virtual ICollection<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();
}
