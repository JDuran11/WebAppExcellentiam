using ExcellentiamCrud.Shared;

namespace ExcellentiamCrud.Server.Repositories
{
    public interface IEstudianteRepositoryPort
    {
        Task<List<EstudianteDTO>> ObtenerEstudiantes(string nombrePatron = "");
        Task<EstudianteDTO> ObtenerEstudiantePorId(int id);
        Task<string> AgregarEstudiante(EstudianteDTO estudianteDto);
        Task<string> ActualizarEstudiante(EstudianteDTO estudianteDto);
        Task<string> EliminarEstudiante(int id);
        Task<byte[]> DescargarEstudiantesExcel(int cursoId);

    }
}
