using ExcellentiamCrud.Shared;
using System.Threading.Tasks;

namespace ExcellentiamCrud.Server.Repositories
{
    public interface ICursoRepositoryPort
    {
        Task<List<CursoDTO>> ObtenerCursos(string nombrePatron = "");
        Task<CursoDTO> ObtenerCursoPorId(int id);
        Task<string> GuardarCurso(CursoDTO cursoDto);
        Task<string> ActualizarCurso(CursoDTO cursoDto);
        Task<string> EliminarCurso(int id);
        Task<byte[]> DescargarCursosExcel();
    }
}
