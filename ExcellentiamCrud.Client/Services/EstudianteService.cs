using ExcellentiamCrud.Shared;
using System.Net.Http.Json;

namespace ExcellentiamCrud.Client.Services
{
    public class EstudianteService : IEstudianteService
    {
        private readonly HttpClient _http;

        public EstudianteService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<EstudianteDTO>> getEstudiantes(string nombrePatron = "")
        {
            var result = await _http.GetFromJsonAsync<ResponseAPI<List<EstudianteDTO>>>("api/estudiante/getEstudiantes");

            if (result!.Success)
                return result.Value!;
            else
                throw new Exception(result.Message);
        }
        public async Task<EstudianteDTO> ObtenerEstudiantePorId(int id)
        {
            var result = await _http.GetFromJsonAsync<ResponseAPI<EstudianteDTO>>($"api/estudiante/getEstudiante/{id}");

            if (result!.Success)
                return result.Value!;
            else
                throw new Exception(result.Message);
        }

        public async Task<string> AgregarEstudiante(EstudianteDTO estudianteDto)
        {
            var response = await _http.PostAsJsonAsync("api/estudiante/agregarEstudiante", estudianteDto);
            var result = await response.Content.ReadFromJsonAsync<ResponseAPI<string>>();

            if (result!.Success)
                return result.Value!;
            else
                throw new Exception(result.Message);
        }

        public async Task<string> ActualizarEstudiante(EstudianteDTO estudianteDto)
        {
            var response = await _http.PutAsJsonAsync("api/estudiante/actualizarEstudiante", estudianteDto);
            var result = await response.Content.ReadFromJsonAsync<ResponseAPI<string>>();

            if (result!.Success)
                return result.Value!;
            else
                throw new Exception(result.Message);
        }

        public async Task<string> EliminarEstudiante(int id)
        {
            var response = await _http.DeleteAsync($"api/estudiante/eliminarEstudiante/{id}");
            var result = await response.Content.ReadFromJsonAsync<ResponseAPI<string>>();

            if (result!.Success)
                return result.Value!;
            else
                throw new Exception(result.Message);
        }

    }
}
