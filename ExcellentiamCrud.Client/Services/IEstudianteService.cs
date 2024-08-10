using ExcellentiamCrud.Shared;

namespace ExcellentiamCrud.Client.Services
{
    public interface IEstudianteService
    {
        Task<List<EstudianteDTO>> getEstudiantes(string nombrePatron = "");
        Task<EstudianteDTO> ObtenerEstudiantePorId(int id);
        Task<string> AgregarEstudiante(EstudianteDTO estudianteDto);
        Task<string> ActualizarEstudiante(EstudianteDTO estudianteDto);
        Task<string> EliminarEstudiante(int id);
    }
}
