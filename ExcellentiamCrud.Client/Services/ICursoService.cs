using ExcellentiamCrud.Shared;

namespace ExcellentiamCrud.Client.Services
{
    public interface ICursoService
    {
        Task<List<CursoDTO>> getCursos(string nombrePatron = "");
        Task<CursoDTO> ObtenerCursoPorId(int id);
        Task<string> GuardarCurso(CursoDTO cursoDto);
        Task<string> ActualizarCurso(CursoDTO cursoDto);
        Task<string> EliminarCurso(int id);
        Task<byte[]> DescargarCursosExcel();
        Task<byte[]> DescargarEstudiantesExcel(int cursoId);
    }
}
